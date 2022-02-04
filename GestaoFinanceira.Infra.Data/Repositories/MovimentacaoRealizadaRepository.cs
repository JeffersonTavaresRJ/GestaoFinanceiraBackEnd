using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class MovimentacaoRealizadaRepository : GenericRepository<MovimentacaoRealizada>, IMovimentacaoRealizadaRepository
    {
        public MovimentacaoRealizadaRepository(SqlContext sqlContext):base(sqlContext)
        {

        }
        
        public override IEnumerable<MovimentacaoRealizada> GetAll(int idUsuario)
        {
            return dbset.Where(mr=>mr.Conta.IdUsuario == idUsuario);
        }

        public override void Add(MovimentacaoRealizada obj)
        {
            context.Entry(obj).State = EntityState.Added;
            context.Entry(obj).Reference(mr => mr.Movimentacao).IsModified = false;
            context.SaveChanges();            
        }

        public override void Update(MovimentacaoRealizada obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            context.Entry(obj).Reference(mr => mr.Movimentacao).IsModified = false;
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
            return dbset.Where(mr=>mr.Id==id)
                        .Include(mr => mr.Movimentacao).ThenInclude(x=>x.ItemMovimentacao).ThenInclude(x=>x.Categoria)
                        .Include(mr => mr.Movimentacao.MovimentacaoPrevista).FirstOrDefault();
        }

        public IEnumerable<MovimentacaoRealizada> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia)
        {
            return dbset.Where(mr =>  mr.DataReferencia == dataReferencia && mr.IdItemMovimentacao == idItemMovimentacao)
                        .Include(mr => mr.Movimentacao.ItemMovimentacao)
                        .Include(mr => mr.Movimentacao.ItemMovimentacao.Categoria)
                        .Include(mr => mr.Movimentacao.MovimentacaoPrevista);
        }
    }
}
