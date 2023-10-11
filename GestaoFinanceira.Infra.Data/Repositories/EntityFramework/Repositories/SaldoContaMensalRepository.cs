using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class SaldoContaMensalRepository : ISaldoContaMensalRepository
    {
        protected SqlContext context;
        protected DbSet<SaldoContaMensal> dbset;

        public SaldoContaMensalRepository(SqlContext context)
        {
            this.context = context;
            dbset = this.context.Set<SaldoContaMensal>();
        }

        public async Task<IEnumerable<SaldoContaMensal>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal)
        {
            return await dbset.Where(x => x.IdUsuario == idUsuario && x.Ano >= anoInicial && x.Ano <= anoFinal).ToListAsync();
        }
    }
}
