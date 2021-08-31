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

                MovimentacaoPrevista movimentacaoPrevista = unitOfWork.IMovimentacaoPrevistaRepository
                                                                          .GetByKey(movimentacoesRealizadas[0].IdItemMovimentacao,
                                                                                    movimentacoesRealizadas[0].DataReferencia);
                
                if (movimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.A
                 && movimentacoesRealizadas.Sum(x => x.Valor) >= movimentacaoPrevista.Valor)
                {
                     movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.Q;
                     unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacaoPrevista);
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

        public void Update(MovimentacaoRealizada movimentacaoRealizada)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRealizadaRepository.Add(movimentacaoRealizada);
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

        public void Delete(MovimentacaoRealizada movimentacaoRealizada)
        {
            try
            {
                unitOfWork.BeginTransaction();

                unitOfWork.IMovimentacaoRealizadaRepository.Delete(movimentacaoRealizada);

                MovimentacaoPrevista movimentacaoPrevista = unitOfWork.IMovimentacaoPrevistaRepository.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                                movimentacaoRealizada.DataReferencia);

                if(movimentacaoPrevista.Status == Models.Enuns.StatusMovimentacaoPrevista.Q)
                {
                    List<MovimentacaoRealizada> movimentacoesRealizadas = unitOfWork.IMovimentacaoRealizadaRepository
                                                                                    .GetByDataReferencia(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                         movimentacaoRealizada.DataReferencia)
                                                                                    .ToList();

                    if (movimentacoesRealizadas.Sum(x => x.Valor) < movimentacaoPrevista.Valor)
                    {
                        movimentacaoPrevista.Status = Models.Enuns.StatusMovimentacaoPrevista.A;
                        unitOfWork.IMovimentacaoPrevistaRepository.Update(movimentacaoPrevista);
                    }
                }                
                unitOfWork.Commit();
            }
            //catch (MovPrevAlteraStatus e)
            //{
   
            //} 
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
