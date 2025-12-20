using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class MovimentacaoRealizadaRepository : GenericRepository<MovimentacaoRealizada>, IMovimentacaoRealizadaRepository
    {
        public MovimentacaoRealizadaRepository(SqlContext sqlContext) : base(sqlContext)
        {

        }

        public override IEnumerable<MovimentacaoRealizada> GetAll(int idUsuario)
        {
            return dbset.Where(mr => mr.Conta.IdUsuario == idUsuario);
        }

        public override int Add(MovimentacaoRealizada obj)
        {
            context.Entry(obj).State = EntityState.Added;
            context.Entry(obj).Reference(mr => mr.Movimentacao).IsModified = false;
            context.SaveChanges();
            return obj.Id;
        }

        public override void Update(MovimentacaoRealizada obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            context.Entry(obj).Reference(mr => mr.Movimentacao).IsModified = false;
            context.Entry(obj).Property(mr => mr.IdMovimentacaoPrevista).IsModified = false;
            context.SaveChanges();
        }

        public override void Delete(MovimentacaoRealizada obj)
        {
            context.Entry(obj).State = EntityState.Deleted;
            context.Entry(obj).Reference(mr => mr.Movimentacao).IsModified = false;
            context.SaveChanges();
        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(mr => mr.Conta.IdUsuario == idUsuario));
        }

        public override MovimentacaoRealizada GetId(int id)
        {
            return dbset.Where(mr => mr.Id == id)
                        .Include(mr => mr.Movimentacao).ThenInclude(x => x.ItemMovimentacao).ThenInclude(x => x.Categoria)
                        .Include(mr => mr.Conta)
                        .Include(mr => mr.FormaPagamento)
                        .Include(mr => mr.Movimentacao.MovimentacoesPrevistas).FirstOrDefault();
        }

        public IEnumerable<MovimentacaoRealizada> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia)
        {
            return dbset.Where(mr => mr.DataReferencia == dataReferencia && mr.IdItemMovimentacao == idItemMovimentacao)
                        .Include(mr => mr.Movimentacao.ItemMovimentacao)
                        .Include(mr => mr.Movimentacao.ItemMovimentacao.Categoria)
                        .Include(mr => mr.Movimentacao.MovimentacoesPrevistas)
                        .Include(mr => mr.Conta)
                        .Include(mr => mr.FormaPagamento);
        }

        public IEnumerable<MovimentacaoRealizada> GetByUsuario(int idUsuario, DateTime dataReferencia)
        {
            return dbset.Where(mr => mr.DataReferencia == dataReferencia && mr.Conta.IdUsuario == idUsuario)
                        .Include(mr => mr.Movimentacao.ItemMovimentacao)
                        .Include(mr => mr.Movimentacao.ItemMovimentacao.Categoria)
                        .Include(mr => mr.Movimentacao.MovimentacoesPrevistas)
                        .Include(mr => mr.Conta)
                        .Include(mr => mr.FormaPagamento);
        }
    }
}
