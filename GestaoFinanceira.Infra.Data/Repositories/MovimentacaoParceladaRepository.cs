using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class MovimentacaoParceladaRepository : IMovimentacaoParceladaRepository
    {
        private SqlContext context;
        private DbSet<MovimentacaoParcelada> dbset;

        public MovimentacaoParceladaRepository(SqlContext context)
        {
            this.context = context;
            this.dbset = context.Set<MovimentacaoParcelada>();
        }

        public void Add(MovimentacaoParcelada obj)
        {
            dbset.Add(obj);
            context.SaveChanges();
        }

        public void Delete(MovimentacaoParcelada obj)
        {
            dbset.Remove(obj);
            context.SaveChanges();
        }

        public void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(mp=>mp.Movimentacao.ItemMovimentacao.Categoria.IdUsuario.Equals(idUsuario)));
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
