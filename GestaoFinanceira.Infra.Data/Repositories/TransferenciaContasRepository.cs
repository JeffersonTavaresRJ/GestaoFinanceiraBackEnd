using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class TransferenciaContasRepository: ITransferenciaContasRepository
    {
        protected SqlContext context;
        protected DbSet<IQueryable<int>> dbset;

        public TransferenciaContasRepository(SqlContext context)
        {
            this.context = context;
            dbset = this.context.Set<IQueryable<int>>();
        }

        public IEnumerable<int> Executar(TransferenciaContas transferenciaContas)
        {

            try
            {
                return (IEnumerable<int>)this.context.Set<IEnumerable<int>>()
                    .FromSqlRaw($" EXECUTE PRC_TRANSFERENCIA_ENTRE_CONTAS @pIdCont, @pIdContDestino, @pDataMore, @pValorMore, @pObservacao",
                new SqlParameter("@pIdCont", transferenciaContas.IdConta), 
                new SqlParameter("@pIdContDestino", transferenciaContas.IdContaDestino), 
                new SqlParameter("@pDataMore", transferenciaContas.DataMovimentacaoRealizada),
                new SqlParameter("@pValorMore", transferenciaContas.Valor),
                new SqlParameter("@pObservacao", transferenciaContas.Observacao)).ToList();

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            
        }

      
    }
}
