using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class SaldoAnualRepository : ISaldoAnualRepository
    {
        protected SqlContext context;
        protected DbSet<SaldoAnual> dbset;

        public SaldoAnualRepository(SqlContext context)
        {
            this.context = context;
            dbset = this.context.Set<SaldoAnual>();
        }

        public async Task<IEnumerable<SaldoAnual>> GetSaldoAnual(int idUsuario, int anoInicial, int anoFinal)
        {
            return await dbset.Where(x=>x.IdUsuario==idUsuario && x.Ano >=anoInicial && x.Ano <= anoFinal).ToListAsync();
        }        
    }
}
