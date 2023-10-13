using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class ItemMovimentacaoRepository : GenericRepository<ItemMovimentacao>, IItemMovimentacaoRepository
    {
        public ItemMovimentacaoRepository(SqlContext sqlContext) : base(sqlContext)
        {

        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(i => i.Categoria.IdUsuario == idUsuario));
        }

        public override IEnumerable<ItemMovimentacao> GetAll(int idUsuario)
        {
            return dbset.Where(i => i.Categoria.IdUsuario == idUsuario);
        }
    }
}
