using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
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

        public List<MovimentacaoPrevista> AddResult(List<MovimentacaoPrevista> movimentacoesPrevistas)
        {
            List<MovimentacaoPrevista> result = new List<MovimentacaoPrevista> ();
            try
            {
                unitOfWork.BeginTransaction();
                movimentacoesPrevistas = movimentacoesPrevistas.OrderBy(mp=>mp.NrParcela).ToList();
                int? idMovPrevParcelada = null;

                foreach (MovimentacaoPrevista movimentacaoPrevista in movimentacoesPrevistas)
                {
                    Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                                            movimentacaoPrevista.DataReferencia);
                    if (movimentacao == null)
                    {
                        unitOfWork.IMovimentacaoRepository.Add(movimentacaoPrevista.Movimentacao);
                    }
                    
                    unitOfWork.IMovimentacaoPrevistaRepository.Add(movimentacaoPrevista);                    

                    if(movimentacaoPrevista.NrParcela == 1)
                    {
                        idMovPrevParcelada = movimentacaoPrevista.Id;
                    }

                    movimentacaoPrevista.IdMovPrevParcelada = idMovPrevParcelada;

                    MovimentacaoPrevista movPrev = unitOfWork.IMovimentacaoPrevistaRepository.GetId(movimentacaoPrevista.Id);
                    result.Add(movPrev);
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
            return result;

        }

        public MovimentacaoPrevista UpdateResult(MovimentacaoPrevista obj)
        {
            MovimentacaoPrevista movPrev;
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.IMovimentacaoRepository.Update(obj.Movimentacao);

                //Tratamento para não alterar o número de parcelas..
                var movimentacaoPrevista = unitOfWork.IMovimentacaoPrevistaRepository.GetId(obj.Id);
                obj.NrParcela = movimentacaoPrevista.NrParcela;
                obj.NrParcelaTotal = movimentacaoPrevista.NrParcelaTotal;

                unitOfWork.IMovimentacaoPrevistaRepository.Update(obj);
                unitOfWork.Commit();

                //Preenchimento de todas as propriedades para atualização do MongoDB..
                movPrev = unitOfWork.IMovimentacaoPrevistaRepository.GetId(obj.Id);

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
            return movPrev;
        }

        public void Delete(MovimentacaoPrevista obj, out List<MovimentacaoPrevista> movimentacaoPrevistas)
        {
            try
            {
                unitOfWork.BeginTransaction();
                List<MovimentacaoPrevista> listaMovPrevistas = new List<MovimentacaoPrevista>();
                
                unitOfWork.IMovimentacaoPrevistaRepository.Delete(obj);

                Movimentacao movimentacao = unitOfWork.IMovimentacaoRepository.GetByKey(obj.IdItemMovimentacao, obj.DataReferencia);
                if ( movimentacao.MovimentacoesRealizadas.Count + movimentacao.MovimentacoesPrevistas.Count == 0)
                {
                    unitOfWork.IMovimentacaoRepository.Delete(movimentacao);
                }

                if (obj.NrParcelaTotal > 1)
                {
                    obj.NrParcelaTotal--;
                    DateTime dataIni = obj.NrParcela == 1 ? obj.DataReferencia : obj.DataReferencia.AddMonths(obj.NrParcelaTotal*-1);
                    DateTime dataFim = obj.NrParcela == 1 ? obj.DataReferencia.AddMonths(obj.NrParcelaTotal) : obj.DataReferencia;

                    listaMovPrevistas = unitOfWork.IMovimentacaoPrevistaRepository
                                                  .GetByMovPrevParcelada((int)obj.IdMovPrevParcelada)
                                                  .OrderBy(mp => mp.DataReferencia).ToList();

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

        public MovimentacaoPrevista GetId(int id)
        {
            return unitOfWork.IMovimentacaoPrevistaRepository.GetId(id);
        }
    }
}
