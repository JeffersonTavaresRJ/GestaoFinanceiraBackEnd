using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class MovimentacaoPrevistaRepository : GenericRepository<MovimentacaoPrevista>, IMovimentacaoPrevistaRepository
    {
        public MovimentacaoPrevistaRepository(SqlContext sqlContext) : base(sqlContext)
        {

        }

        public override int Add(MovimentacaoPrevista obj)
        {
            context.Entry(obj).State = EntityState.Added;
            context.Entry(obj).Reference(mr => mr.Movimentacao).IsModified = false;
            context.SaveChanges();
            return 0;
        }

        public override void Update(MovimentacaoPrevista obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            //context.Entry(obj).Property(mp => mp.NrParcela).IsModified = obj.NrParcelaTotal > 1? true:false;
            //context.Entry(obj).Property(mp => mp.NrParcelaTotal).IsModified = obj.NrParcelaTotal > 1 ? true : false;
            context.SaveChanges();
        }

        public override void Delete(MovimentacaoPrevista obj)
        {
            context.Entry(obj).State = EntityState.Deleted;
            context.Entry(obj).Reference(mp => mp.Movimentacao).IsModified = false;
            context.SaveChanges();
        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(mp => mp.FormaPagamento.IdUsuario == idUsuario));
        }

        public IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, 
                                                                     int? idItemMovimentacao, DateTime dataRefIni, DateTime dataRefFim)
        {
            return dbset.Where(mp => mp.FormaPagamento.IdUsuario == idUsuario &&
                                          (mp.IdItemMovimentacao == idItemMovimentacao ||
                                          idItemMovimentacao == null) &&
                                          mp.DataReferencia >= dataRefIni &&
                                          mp.DataReferencia <= dataRefFim)
                        .Include(mp => mp.Movimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao.Categoria).ToList();
        }

        public IEnumerable<MovimentacaoPrevista> GetByMovPrevParcelada(int idMovPrevParcelada)
        {
            return dbset.Where(mp => mp.IdMovPrevParcelada == idMovPrevParcelada)
                        .Include(mp => mp.Movimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao.Categoria).ToList();
        }

        public override IEnumerable<MovimentacaoPrevista> GetAll(int idUsuario)
        {
            return dbset.Where(mp => mp.FormaPagamento.IdUsuario == idUsuario);
        }

        public IEnumerable<MovimentacaoPrevista> GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            return dbset.Where(mp => mp.IdItemMovimentacao == idItemMovimentacao &&
                               mp.DataReferencia == dataReferencia)
                        .Include(mp => mp.Movimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao.Categoria).ToList();

        }

        public override MovimentacaoPrevista GetId(int id)
        {
            return dbset.Where(mp => mp.Id == id)
                        .Include(mp => mp.Movimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao)
                        .Include(mp => mp.Movimentacao.ItemMovimentacao.Categoria)
                        .FirstOrDefault();

        }
    }
}