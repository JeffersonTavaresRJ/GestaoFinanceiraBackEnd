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
    public class SaldoDiarioRepository : ISaldoDiarioRepository
    {
        protected SqlContext context;
        protected DbSet<SaldoDiario> dbset;

        public SaldoDiarioRepository(SqlContext context)
        {
            this.context = context;
            dbset = context.Set<SaldoDiario>();
        }

        public SaldoDiario GetByKey(int idConta, DateTime dataSaldo)
        {
            return dbset.Where(sd => sd.IdConta == idConta && sd.DataSaldo == dataSaldo).FirstOrDefault();
        }

        public IEnumerable<SaldoDiario> GetByPeriodo(int idUsuario, DateTime dataIni, DateTime dataFim)
        {
            return dbset.Include(sa=>sa.Conta)
                        .Where(sa => sa.Conta.IdUsuario==idUsuario && 
                               sa.DataSaldo >= dataIni && 
                               sa.DataSaldo <= dataFim).ToList();
        }

        public IEnumerable<SaldoDiario> GetBySaldosDiario(int idConta, DateTime dataSaldo)
        {
            return dbset.Where(sa => sa.IdConta == idConta && sa.DataSaldo >= dataSaldo).ToList();
        }
    }
}
