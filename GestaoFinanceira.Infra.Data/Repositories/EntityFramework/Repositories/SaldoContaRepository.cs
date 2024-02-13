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
    public class SaldoContaRepository : ISaldoContaRepository
    {
        protected SqlContext context;

        public SaldoContaRepository(SqlContext context)
        {
            this.context = context;            
        }

        public async Task<IEnumerable<SaldoContaAnual>> GetSaldoAnualConta(int idUsuario, int anoInicial, int anoFinal)
        {
           DbSet<SaldoContaAnual> dbset = context.Set<SaldoContaAnual>();
           return await dbset.Where(x=>x.IdUsuario==idUsuario && x.Ano >= anoInicial && x.Ano <=anoFinal).ToListAsync();
        }

        public async Task<IEnumerable<SaldoContaMensal>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal)
        {
            DbSet<SaldoContaMensal> dbset = this.context.Set<SaldoContaMensal>();
            return await dbset.Where(x => x.IdUsuario == idUsuario && x.Ano >= anoInicial && x.Ano <= anoFinal).ToListAsync();
        }
    }
}
