using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class ContaRepository : GenericRepository<Conta>, IContaRepository
    {
        public ContaRepository(SqlContext context):base(context)
        {
                
        }
        
        public override IEnumerable<Conta> GetAll(int idUsuario)
        {
            return dbset.Where(c => c.IdUsuario == idUsuario);
        }

        public void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(c => c.IdUsuario == idUsuario));
        }
    }
}
