using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
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

        public void Add(List<MovimentacaoRealizada> movimentacoesRealizadas)
        {
            try
            {
                unitOfWork.BeginTransaction();
                foreach (MovimentacaoRealizada movimentacaoRealizada in movimentacoesRealizadas)
                {
                    unitOfWork.IMovimentacaoRealizadaRepository.Add(movimentacaoRealizada);
                }

    
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacoesRealizadas[0].IdItemMovimentacao,
                                                                                    movimentacoesRealizadas[0].DataReferencia);


                if (movimentacao.MovimentacaoPrevista!= null && movimentacao.MovimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.A &&
                    movimentacao.MovimentacoesRealizadas.Sum(x => x.Valor) >= movimentacao.MovimentacaoPrevista.Valor)
                {
                     movimentacao.MovimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.Q;
                     unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacao.MovimentacaoPrevista);
                     throw new MovPrevAlteraStatus(movimentacao.MovimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacao.MovimentacaoPrevista.DataReferencia,
                                                     movimentacao.MovimentacaoPrevista.Status);
                }

                unitOfWork.Commit();

            }
            catch (MovPrevAlteraStatus)
            {
                unitOfWork.Commit();
                throw;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }

        public void Update(MovimentacaoRealizada movimentacaoRealizada)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRealizadaRepository.Update(movimentacaoRealizada);

                List<MovimentacaoRealizada> movimentacoesRealizadas = unitOfWork.IMovimentacaoRealizadaRepository
                                                                                .GetByDataReferencia(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                     movimentacaoRealizada.DataReferencia)
                                                                                .ToList();

                MovimentacaoPrevista movimentacaoPrevista = unitOfWork.IMovimentacaoPrevistaRepository
                                                                      .GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                movimentacaoRealizada.DataReferencia);

                if (movimentacaoPrevista!= null && movimentacoesRealizadas.Sum(x => x.Valor) < movimentacaoPrevista.Valor &&
                    movimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.Q)
                {
                    movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
                    unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacaoPrevista);
                    throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                  movimentacaoPrevista.DataReferencia,
                                                  movimentacaoPrevista.Status);
                }
                else if (movimentacaoPrevista!= null && movimentacoesRealizadas.Sum(x => x.Valor) >= movimentacaoPrevista.Valor &&
                         movimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.A)
                {
                    movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.Q;
                    unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacaoPrevista);
                    throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                  movimentacaoPrevista.DataReferencia,
                                                  movimentacaoPrevista.Status);
                }                
                unitOfWork.Commit();

            }
            catch (MovPrevAlteraStatus)
            {
                unitOfWork.Commit();
                throw;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }

        public void Delete(MovimentacaoRealizada movimentacaoRealizada)
        {
            try
            {
                unitOfWork.BeginTransaction();

                unitOfWork.IMovimentacaoRealizadaRepository.Delete(movimentacaoRealizada);

                MovimentacaoPrevista movimentacaoPrevista = unitOfWork.IMovimentacaoPrevistaRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                                movimentacaoRealizada.DataReferencia);

                if(movimentacaoPrevista != null &&  movimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.Q)
                {
                    List<MovimentacaoRealizada> movimentacoesRealizadas = unitOfWork.IMovimentacaoRealizadaRepository
                                                                                    .GetByDataReferencia(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                         movimentacaoRealizada.DataReferencia)
                                                                                    .ToList();

                    if (movimentacoesRealizadas.Sum(x => x.Valor) < movimentacaoPrevista.Valor)
                    {
                        movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
                        unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacaoPrevista);
                        throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao, 
                                                      movimentacaoPrevista.DataReferencia,
                                                      movimentacaoPrevista.Status);
                    }
                }                
                unitOfWork.Commit();
            }
            catch (MovPrevAlteraStatus)
            {
                unitOfWork.Commit();
                throw;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }

        public MovimentacaoRealizada GetId(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                return unitOfWork.IMovimentacaoRealizadaRepository.GetId(id);

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


    }
}
