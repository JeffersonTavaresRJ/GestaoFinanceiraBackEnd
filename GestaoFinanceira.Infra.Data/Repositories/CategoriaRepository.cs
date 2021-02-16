using GestaoFinanceira.Infra.Data.Context;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class CategoriaRepository :GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(SqlContext context):base(context)
        {

        }
        public void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(c => c.IdUsuario == idUsuario));
        }

        public override IEnumerable<Categoria> GetAll(int idUsuario)
        {
            return dbset.Where(c => c.IdUsuario == idUsuario);
        }
    }
}
