using GestaoFinanceira.Domain.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(SqlContext context) : base(context)
        {

        }
        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(c => c.IdUsuario == idUsuario));
        }

        public override IEnumerable<Categoria> GetAll(int idUsuario)
        {
            return dbset.Where(c => c.IdUsuario == idUsuario);
        }
    }
}
