using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class FechamentoRepository: IFechamentoRepository
    {
        protected SqlContext context;

        public FechamentoRepository(SqlContext context)
        {
            this.context = context;
        }

        public void Executar(int idUsuario, DateTime dataReferencia)
        {
            this.context.Database.ExecuteSqlRaw("PRC_GRAVA_FECHAMENTO @pIdUsuario, @pDataReferencia", new SqlParameter("@pIdUsuario", idUsuario), new SqlParameter("@pDataReferencia", dataReferencia));
        }
    }
}
