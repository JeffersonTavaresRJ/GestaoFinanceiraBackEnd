using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Domain.Services
{
    public class MovimentacaoPrevistaDomainService : GenericWriteDomainService<MovimentacaoPrevista>, IMovimentacaoPrevistaDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public MovimentacaoPrevistaDomainService(IUnitOfWork unitOfWork) : base(unitOfWork.IMovimentacaoPrevistaRepository)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Add(List<MovimentacaoPrevista> movimentacoesPrevistas)
        {
            try
            {
                unitOfWork.BeginTransaction();
                foreach (MovimentacaoPrevista movimentacaoPrevista in movimentacoesPrevistas)
                {
                    Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                                            movimentacaoPrevista.DataReferencia);
                    if (movimentacao != null)
                    {
                        unitOfWork.IMovimentacaoRepository.Update(movimentacaoPrevista.Movimentacao);
                    }
                    unitOfWork.IMovimentacaoPrevistaRepository.Add(movimentacaoPrevista);
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

        public override void Update(MovimentacaoPrevista obj)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRepository.Update(obj.Movimentacao);
                unitOfWork.IMovimentacaoPrevistaRepository.Update(obj);
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

        public override void Delete(MovimentacaoPrevista obj)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoPrevistaRepository.Delete(obj);

                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                if (movimentacao.MovimentacoesRealizadas.Count == 0)
                {
                    unitOfWork.IMovimentacaoRepository.Delete(movimentacao);
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

        public List<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? ItemMovimentacaoId, DateTime dataRefIni, DateTime dataRefFim)
        {
            return unitOfWork.IMovimentacaoPrevistaRepository.GetByDataReferencia(idUsuario, ItemMovimentacaoId, dataRefIni, dataRefFim).ToList();
        }

        public MovimentacaoPrevista GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            return unitOfWork.IMovimentacaoPrevistaRepository.GetByKey(idItemMovimentacao, dataReferencia);
        }
    }
}
