using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class ItemMovimentacaoMensalRepository : IItemMovimentacaoMensalRepository
    {
        protected SqlContext context;
        protected DbSet<ItemMovimentacaoMensal> dbset;

        public ItemMovimentacaoMensalRepository(SqlContext context)
        {
            this.context = context;
            dbset = this.context.Set<ItemMovimentacaoMensal>();
        }

        public async Task<IEnumerable<ItemMovimentacaoMensal>> GetItemMovimentacaoMensal(int idUsuario, DateTime dataIncial, DateTime dataFinal)
        {
            IEnumerable<ItemMovimentacaoMensal> result = await dbset.Where(x => x.IdUsuario == idUsuario && x.DataReferencia >= dataIncial && x.DataReferencia <= dataFinal).ToListAsync();
            return result;
        }        
    }
}
