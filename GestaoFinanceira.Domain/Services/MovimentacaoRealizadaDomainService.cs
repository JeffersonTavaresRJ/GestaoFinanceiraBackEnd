using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
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

        public MovimentacaoRealizada Add(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista)
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
                //Tratamento para retorno do método para gravação no MongoDB..
                var id = unitOfWork.IMovimentacaoRealizadaRepository.Add(movimentacaoRealizada);
                movimentacaoRealizada = unitOfWork.IMovimentacaoRealizadaRepository.GetId(id);

                if (movimentacao != null &&
                    movimentacao.MovimentacaoPrevista != null &&
                    movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.A)
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
                        _movimentacaoPrevista = movimentacao.MovimentacaoPrevista;
                    }
                }

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

        public MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista)
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


    }
}
