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

        public void Add(MovimentacaoPrevista obj, int qtdeParcelas)
        {
            try
            {
                List<MovimentacaoPrevista> lista = new List<MovimentacaoPrevista>();
                
                for (int i = 1; i <= qtdeParcelas; i++)
                {
                    MovimentacaoPrevista mov = new MovimentacaoPrevista
                    {
                        Movimentacao = new Movimentacao
                        {
                            DataReferencia = obj.Movimentacao.DataReferencia.AddMonths(i-1),
                            IdItemMovimentacao = obj.Movimentacao.IdItemMovimentacao,
                            ItemMovimentacao = obj.Movimentacao.ItemMovimentacao,
                            MovimentacoesPrevistas = obj.Movimentacao.MovimentacoesPrevistas,
                            MovimentacoesRealizadas = obj.Movimentacao.MovimentacoesRealizadas,
                            Observacao = qtdeParcelas >=2 ? $"{obj.Movimentacao.Observacao} ({i}/{qtdeParcelas})": obj.Movimentacao.Observacao,
                            TipoPrioridade = obj.Movimentacao.TipoPrioridade
                        },
                        DataReferencia = obj.DataReferencia.AddMonths(i-1),
                        DataVencimento = obj.DataVencimento.AddMonths(i-1),
                        FormaPagamento = obj.FormaPagamento,
                        IdFormaPagamento = obj.IdFormaPagamento,
                        IdItemMovimentacao = obj.IdItemMovimentacao,                        
                        Status = obj.Status,
                        Valor = obj.Valor
                    };
                    lista.Add(mov);                    
                }

                unitOfWork.BeginTransaction();
                foreach (MovimentacaoPrevista movimentacaoPrevista in lista)
                {
                    Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                                            movimentacaoPrevista.DataReferencia);

                    if (movimentacao != null)
                    {
                        if (movimentacao.MovimentacoesRealizadas.Count > 0 && movimentacaoPrevista.Status != Models.Enuns.StatusMovimentacaoPrevista.Q)
                        {
                            throw new StatusMovimentacaoInvalidoException(movimentacao.ItemMovimentacao.Descricao,
                                                                          movimentacao.DataReferencia);
                        }
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
                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                if (movimentacao.MovimentacoesRealizadas.Count > 0)
                {
                    obj.Movimentacao = null;
                }
                unitOfWork.IMovimentacaoPrevistaRepository.Delete(obj);
                unitOfWork.Commit();
                //setar movimentação para gravação no caching..
                obj.Movimentacao = movimentacao;
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
