using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Domain.Services
{
    public class MovimentacaoRealizadaDomainService : IMovimentacaoRealizadaDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public MovimentacaoRealizadaDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Add(List<MovimentacaoRealizada> movimentacoesRealizadas, out MovimentacaoPrevista movimentacaoPrevista)
        {
            try
            {
                unitOfWork.BeginTransaction();
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacoesRealizadas[0].IdItemMovimentacao,
                                                                                        movimentacoesRealizadas[0].DataReferencia);

                if(movimentacao == null)
                {
                    unitOfWork.IMovimentacaoRepository.Add(movimentacoesRealizadas[0].Movimentacao);
                }

                foreach (MovimentacaoRealizada movimentacaoRealizada in movimentacoesRealizadas)
                {
                    unitOfWork.IMovimentacaoRealizadaRepository.Add(movimentacaoRealizada);
                }

                movimentacaoPrevista = null;

                if (movimentacao != null && 
                    movimentacao.MovimentacaoPrevista!= null && movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.A &&
                    movimentacao.MovimentacoesRealizadas.Sum(x => x.Valor) >= movimentacao.MovimentacaoPrevista.Valor)
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
                throw new Exception(e.Message);
            }
            finally{
                unitOfWork.Dispose();
            }
        }

        public void Update(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRealizadaRepository.Update(movimentacaoRealizada);

                movimentacaoPrevista = null;
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                        movimentacaoRealizada.DataReferencia);

                if (movimentacao.MovimentacaoPrevista != null && 
                    movimentacao.MovimentacoesRealizadas.Sum(x => x.Valor) < movimentacao.MovimentacaoPrevista.Valor &&
                    movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.Q)
                {
                    movimentacao.MovimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
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
                throw new Exception(e.Message);
            }
            finally
            {
                unitOfWork.Dispose();
            }
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

                if (movimentacao.MovimentacaoPrevista != null && movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.Q)
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
                throw new Exception(e.Message);
            }
            finally
            {
                unitOfWork.Dispose();
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
                throw new Exception(e.Message);
            }
        }


    }
}
