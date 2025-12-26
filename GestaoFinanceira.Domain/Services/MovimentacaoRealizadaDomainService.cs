using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
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
        private readonly ITransferenciaContasRepository transferenciaContasRepository;
        private List<MovimentacaoPrevista> resultMovPrevistas = new List<MovimentacaoPrevista>();

        public MovimentacaoRealizadaDomainService(IUnitOfWork unitOfWork, ITransferenciaContasRepository transferenciaContasRepository)
        {
            this.unitOfWork = unitOfWork;
            this.transferenciaContasRepository = transferenciaContasRepository;
        }

        public MovimentacaoRealizada Add(MovimentacaoRealizada movimentacaoRealizada, out List<MovimentacaoPrevista> movimentacoesPrevistas, string statusMovimentacaoPrevista = null)
        {

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

                //Tratamento do Status da Movimentação Prevista, se houver..
                if (movimentacaoRealizada.IdMovimentacaoPrevista > 0)
                {
                    var movimentacaoPrevista = unitOfWork.IMovimentacaoPrevistaRepository.GetId((int)movimentacaoRealizada.IdMovimentacaoPrevista);
                    AtualizaStatusMovimentacaoPrevista(movimentacaoPrevista, movimentacaoRealizada.Valor);
                }


                //Tratamento para retorno do método para gravação no MongoDB..
                var id = unitOfWork.IMovimentacaoRealizadaRepository.Add(movimentacaoRealizada);
                movimentacaoRealizada = unitOfWork.IMovimentacaoRealizadaRepository.GetId(id);

                

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
            movimentacoesPrevistas = resultMovPrevistas;
            return movimentacaoRealizada;
        }

        public MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out List<MovimentacaoPrevista> movimentacoesPrevistas, string statusMovimentacaoPrevista = null)
        {
            try
            {
                unitOfWork.BeginTransaction();

                unitOfWork.IMovimentacaoRealizadaRepository.Update(movimentacaoRealizada);
                //Tratamento para retorno do método para gravação no MongoDB..
                movimentacaoRealizada = unitOfWork.IMovimentacaoRealizadaRepository.GetId(movimentacaoRealizada.Id);

                //Tratamento do Status da Movimentação Prevista, se houver..
                AtualizaStatusMovimentacaoPrevista(movimentacaoRealizada.MovimentacaoPrevista, 0);

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
            movimentacoesPrevistas = resultMovPrevistas;
            return movimentacaoRealizada;
        }

        public void Delete(MovimentacaoRealizada movimentacaoRealizada, out List<MovimentacaoPrevista> movimentacoesPrevistas)
        {
            try
            {
                unitOfWork.BeginTransaction();

                unitOfWork.IMovimentacaoRealizadaRepository.Delete(movimentacaoRealizada);

                //Tratamento do Status da Movimentação Prevista, se houver..
                AtualizaStatusMovimentacaoPrevista(movimentacaoRealizada.MovimentacaoPrevista, 0);

                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                        movimentacaoRealizada.DataReferencia);

                if(movimentacaoRealizada.MovimentacaoPrevista == null & movimentacao.MovimentacoesRealizadas.Count == 0)
                {
                    unitOfWork.IMovimentacaoRepository.Delete(movimentacao);
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
            movimentacoesPrevistas = resultMovPrevistas;
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

        private void AtualizaStatusMovimentacaoPrevista(MovimentacaoPrevista movimentacaoPrevista, double valorPago)
        {
            if(movimentacaoPrevista != null)
            {
                var valorTotalPago = unitOfWork.IMovimentacaoPrevistaRepository.GetValorTotalPago(movimentacaoPrevista.Id);

                if (movimentacaoPrevista.MovimentacoesRealizadas == null ||
                   (movimentacaoPrevista.Valor > valorTotalPago + valorPago))
                {
                    movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
                }
                else
                {
                    movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.Q;
                }

                unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacaoPrevista);

                resultMovPrevistas.Add(movimentacaoPrevista);
            }
        }
        
    }
}
