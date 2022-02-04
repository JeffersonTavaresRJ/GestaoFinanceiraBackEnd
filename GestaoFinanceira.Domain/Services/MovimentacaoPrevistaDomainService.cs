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
                        movimentacao.Observacao = movimentacaoPrevista.Movimentacao.Observacao;
                        movimentacao.TipoPrioridade = movimentacaoPrevista.Movimentacao.TipoPrioridade;

                        unitOfWork.IMovimentacaoRepository.Update(movimentacao);
                    }
                    MovimentacaoPrevista movPrev = unitOfWork.IMovimentacaoPrevistaRepository.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                                                       movimentacaoPrevista.DataReferencia);
                    if(movPrev != null)
                    {
                        throw new MovPrevExisteException(movPrev.Movimentacao.ItemMovimentacao.Descricao,
                                                                      movPrev.DataReferencia);
                    }
                    unitOfWork.IMovimentacaoPrevistaRepository.Add(movimentacaoPrevista);
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
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        public void Delete(MovimentacaoPrevista obj, out List<MovimentacaoPrevista> movimentacaoPrevistas)
        {
            try
            {
                unitOfWork.BeginTransaction();
                List<MovimentacaoPrevista> listaMovPrevistas = new List<MovimentacaoPrevista>();
                
                unitOfWork.IMovimentacaoPrevistaRepository.Delete(obj);

                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                if (movimentacao.MovimentacoesRealizadas.Count == 0)
                {
                    unitOfWork.IMovimentacaoRepository.Delete(movimentacao);
                }

                if (obj.NrParcelaTotal > 1)
                {
                    obj.NrParcelaTotal--;
                    DateTime dataIni = obj.NrParcela == 1 ? obj.DataReferencia : obj.DataReferencia.AddMonths(obj.NrParcelaTotal*-1);
                    DateTime dataFim = obj.NrParcela == 1 ? obj.DataReferencia.AddMonths(obj.NrParcelaTotal) : obj.DataReferencia;

                    listaMovPrevistas = unitOfWork.IMovimentacaoPrevistaRepository.GetByDataReferencia(
                        obj.Movimentacao.ItemMovimentacao.Categoria.IdUsuario,
                        obj.IdItemMovimentacao,
                        dataIni,
                        dataFim).OrderBy(mp => mp.DataReferencia).ToList();

                    int parcela = 0;
                    foreach (MovimentacaoPrevista movPrevista in listaMovPrevistas)
                    {
                        movPrevista.NrParcela = ++parcela;
                        movPrevista.NrParcelaTotal = obj.NrParcelaTotal;
                        unitOfWork.IMovimentacaoPrevistaRepository.Update(movPrevista);
                    }

                }

                unitOfWork.Commit();
                movimentacaoPrevistas = listaMovPrevistas;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
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
