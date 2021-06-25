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

        public void Add(MovimentacaoPrevista obj, int qtdeParcelas)
        {
            try
            {
                unitOfWork.BeginTransaction();
                for (int i = 0; i <= qtdeParcelas; i++)
                {
                    obj.DataReferencia = obj.DataReferencia.AddMonths(i);
                    obj.DataVencimento = obj.DataVencimento.AddMonths(i);
                    obj.Movimentacao.DataReferencia = obj.DataReferencia;

                    var mov = unitOfWork.IMovimentacaoRepository.GetByKey(obj.Movimentacao.IdItemMovimentacao, obj.Movimentacao.DataReferencia);
                    if (mov != null)
                    {                    
                        unitOfWork.IMovimentacaoRepository.Update(obj.Movimentacao);
                    }
                    unitOfWork.IMovimentacaoPrevistaRepository.Add(obj);                    
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
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                if (movimentacao.MovimentacoesPrevistas.Count == 1 &&
                    movimentacao.MovimentacoesRealizadas.Count == 0)
                {
                    unitOfWork.IMovimentacaoRepository.Delete(movimentacao);
                }
                unitOfWork.IMovimentacaoPrevistaRepository.Delete(obj);
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
