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
                    /*Tratamento para evitar duplicidade de inserção e erro na gravação da tabela Movimentacao,
                     pois a tabela de movimentacao já pode ter sido gravada na classe MovimentacaoRealizada..*/
                    Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                    if (movimentacao != null)
                    {
                        obj.Movimentacao = null;
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
                /*Tratamento para tratar erro de exclusão da tabela Movimentacao que possuir 
                 * dados cadastrados na tabela Movimentacao Realizada..*/
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                if (movimentacao.MovimentacoesRealizadas == null)
                {
                    obj.Movimentacao = movimentacao;
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

        public List<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idItemMovimentacao, DateTime dataReferenciaInicial, DateTime dataReferenciaFinal)
        {
            return unitOfWork.IMovimentacaoPrevistaRepository.GetByDataReferencia(idUsuario, idItemMovimentacao, dataReferenciaInicial, dataReferenciaFinal).ToList();
        }
    }
}
