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
    public class ItemMovimentacaoRepository : GenericRepository<ItemMovimentacao>, IItemMovimentacaoRepository
    {
        public ItemMovimentacaoRepository(SqlContext sqlContext):base(sqlContext)
        {
          
        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(i=>i.Categoria.IdUsuario==idUsuario));
        }

        public override IEnumerable<ItemMovimentacao> GetAll(int idUsuario)
        {
            return dbset.Where(i => i.Categoria.IdUsuario == idUsuario);
        }
    }
}
