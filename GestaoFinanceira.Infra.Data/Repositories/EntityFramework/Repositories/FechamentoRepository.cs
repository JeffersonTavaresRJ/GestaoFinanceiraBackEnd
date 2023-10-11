using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class FechamentoRepository : IFechamentoRepository
    {
        protected SqlContext context;

        public FechamentoRepository(SqlContext context)
        {
            this.context = context;
        }

        public void Executar(int idUsuario, DateTime dataReferencia, string status)
        {
            try
            {
                context.Database.ExecuteSqlRaw("PRC_GRAVA_FECHAMENTO @pIdUsuario, @pDataReferencia, @pStatus",
                new SqlParameter("@pIdUsuario", idUsuario), new SqlParameter("@pDataReferencia", dataReferencia), new SqlParameter("@pStatus", status));
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }
    }
}
