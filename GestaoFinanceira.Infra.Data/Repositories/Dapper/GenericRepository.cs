using Dapper;
using GestaoFinanceira.Domain.Models.Enuns;
using GestaoFinanceira.Infra.Data.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Data;

namespace GestaoFinanceira.Infra.Data.Repositories.Dapper
{
    public abstract class GenericRepository : IGenericRepository
    {
        protected IDbConnection connection;
        private CommandType commandType;

        protected GenericRepository(IDbConnection connection)
        {
            this.connection = connection;
            commandType = CommandType.Text;
        }

        public IEnumerable<dynamic> Execute(string sqlText, object parameters, TipoExecucao? tipo)
        {
            try
            {
                if (tipo.Equals(TipoExecucao.StoredProcedure))
                {
                    commandType = CommandType.StoredProcedure;
                }

                var result = connection.Query(sqlText, parameters, commandType: commandType);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
