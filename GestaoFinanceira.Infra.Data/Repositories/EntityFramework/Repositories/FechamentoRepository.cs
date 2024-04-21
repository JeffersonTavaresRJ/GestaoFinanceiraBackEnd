using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class FechamentoRepositoryEF : IFechamentoRepository
    {
        protected SqlContext context;

        public FechamentoRepositoryEF(SqlContext context)
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

        public List<FechamentoMensalDTO> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
