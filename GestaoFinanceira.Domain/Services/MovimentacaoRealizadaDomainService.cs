﻿using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Domain.Services
{
    public class MovimentacaoRealizadaDomainService : IMovimentacaoRealizadaDomainService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITransferenciaContasRepository transferenciaContasRepository;

        public MovimentacaoRealizadaDomainService(IUnitOfWork unitOfWork, ITransferenciaContasRepository transferenciaContasRepository)
        {
            this.unitOfWork = unitOfWork;
            this.transferenciaContasRepository = transferenciaContasRepository;
        }

        public MovimentacaoRealizada Add(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista, string statusMovimentacaoPrevista = null)
        {
            MovimentacaoPrevista _movimentacaoPrevista = null;

            try
            {
                unitOfWork.BeginTransaction();
                Movimentacao movimentacao = null;
                movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                           movimentacaoRealizada.DataReferencia);

                if (movimentacao == null)
                {
                    unitOfWork.IMovimentacaoRepository.Add(movimentacaoRealizada.Movimentacao);
                }
                else
                {
                    //movimentacao.Observacao = movimentacaoRealizada.Movimentacao.Observacao;
                    movimentacao.TipoPrioridade = movimentacaoRealizada.Movimentacao.TipoPrioridade;

                    unitOfWork.IMovimentacaoRepository.Update(movimentacao);
                }
                //Tratamento para retorno do método para gravação no MongoDB..
                var id = unitOfWork.IMovimentacaoRealizadaRepository.Add(movimentacaoRealizada);
                movimentacaoRealizada = unitOfWork.IMovimentacaoRealizadaRepository.GetId(id);

                _movimentacaoPrevista = AtualizaStatusMovimentacaoPrevista(movimentacaoRealizada, movimentacao, statusMovimentacaoPrevista);

                unitOfWork.Commit();

            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            finally{
                //unitOfWork.Dispose();
            }
            movimentacaoPrevista = _movimentacaoPrevista;
            return movimentacaoRealizada;
        }

        public MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista, string statusMovimentacaoPrevista = null)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRealizadaRepository.Update(movimentacaoRealizada);
                //Tratamento para retorno do método para gravação no MongoDB..
                movimentacaoRealizada = unitOfWork.IMovimentacaoRealizadaRepository.GetId(movimentacaoRealizada.Id);

                movimentacaoPrevista = null;
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                        movimentacaoRealizada.DataReferencia);

                if (movimentacao.MovimentacaoPrevista != null && 
                    movimentacao.MovimentacoesRealizadas.Sum(x => x.Valor) < movimentacao.MovimentacaoPrevista.Valor)
                {
                    if (statusMovimentacaoPrevista != null && movimentacao.MovimentacaoPrevista.Status != Models.Enuns.StatusMovimentacaoPrevista.Q)
                    {
                        movimentacao.MovimentacaoPrevista.Status = (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), statusMovimentacaoPrevista);
                    }
                    else if(movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.Q)
                    {
                        movimentacao.MovimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
                    }
                    
                    unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacao.MovimentacaoPrevista);
                    movimentacaoPrevista = movimentacao.MovimentacaoPrevista;

                }
                else if (movimentacao.MovimentacaoPrevista != null && 
                         movimentacao.MovimentacoesRealizadas.Sum(x => x.Valor) >= movimentacao.MovimentacaoPrevista.Valor &&
                         movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.A)
                {
                    movimentacao.MovimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.Q;
                    unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacao.MovimentacaoPrevista);
                    movimentacaoPrevista = movimentacao.MovimentacaoPrevista;
                }
                unitOfWork.Commit();

            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            finally
            {
                //unitOfWork.Dispose();
            }
            return movimentacaoRealizada;
        }

        public void Delete(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRealizadaRepository.Delete(movimentacaoRealizada);

                
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                        movimentacaoRealizada.DataReferencia);

                if(movimentacao.MovimentacaoPrevista == null && movimentacao.MovimentacoesRealizadas.Count ==0)
                {
                    unitOfWork.IMovimentacaoRepository.Delete(movimentacao);
                }

                movimentacaoPrevista = null;

                if (movimentacao.MovimentacaoPrevista != null)
                {

                    if (movimentacao.MovimentacoesRealizadas.Sum(x => x.Valor) < movimentacao.MovimentacaoPrevista.Valor)
                    {
                        movimentacao.MovimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
                        unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacao.MovimentacaoPrevista);
                        movimentacaoPrevista = movimentacao.MovimentacaoPrevista;
                    }
                }
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            finally
            {
                //unitOfWork.Dispose();
            }
        }

        public MovimentacaoRealizada GetId(int id)
        {
            try
            {
                return unitOfWork.IMovimentacaoRealizadaRepository.GetId(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException != null? e.InnerException.Message : e.Message);
            }
        }

        public List<MovimentacaoRealizada> GetByUsuario(int idUsuario, DateTime dataReferencia)
        {
            try
            {
                return unitOfWork.IMovimentacaoRealizadaRepository.GetByUsuario(idUsuario, dataReferencia).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public List<MovimentacaoRealizada> ExecutarTransferencia(TransferenciaContas transferenciaConta)
        {
            List<MovimentacaoRealizada> movimentacoesRealizadas = new List<MovimentacaoRealizada>();
            try
            {
                unitOfWork.BeginTransaction();

                var ids = transferenciaContasRepository.Execute(transferenciaConta);

                foreach (var item in ids)
                {
                    MovimentacaoRealizada movimentacaoRealizada = unitOfWork.IMovimentacaoRealizadaRepository.GetId(item.ID);
                    movimentacoesRealizadas.Add(movimentacaoRealizada);
                }

                return movimentacoesRealizadas;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            
        }

        private MovimentacaoPrevista AtualizaStatusMovimentacaoPrevista(MovimentacaoRealizada movimentacaoRealizada, Movimentacao movimentacao, string statusMovimentacaoPrevista=null)
        {
            MovimentacaoPrevista movimentacaoPrevista = null;

            if (movimentacao != null && movimentacao.MovimentacaoPrevista != null)
            {
                //pegando o valor total de lançamentos realizados..
                double valorTotalMovReal = unitOfWork.IMovimentacaoRealizadaRepository.GetByDataReferencia(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                           movimentacaoRealizada.DataReferencia)
                                                                                      .Sum(more => more.Valor);

                //se o valor total de lançamentos realizados for maior ou igual ao valor previsto, quitar movimentação prevista..
                if (valorTotalMovReal >= movimentacao.MovimentacaoPrevista.Valor)
                {
                    movimentacao.MovimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.Q;
                    unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacao.MovimentacaoPrevista);
                    movimentacaoPrevista = movimentacao.MovimentacaoPrevista;

                }else if(statusMovimentacaoPrevista != null) //tratamento para reabrir ou encerrar a Movimentação Prevista no mês..
                {                    
                    movimentacao.MovimentacaoPrevista.Status = (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), statusMovimentacaoPrevista);
                    unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacao.MovimentacaoPrevista);
                    movimentacaoPrevista = movimentacao.MovimentacaoPrevista;
                }
            }

            return movimentacaoPrevista;
        }
    }
}
