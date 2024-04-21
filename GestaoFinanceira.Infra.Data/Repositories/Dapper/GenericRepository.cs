using Dapper;
using GestaoFinanceira.Infra.Data.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace GestaoFinanceira.Infra.Data.Repositories.Dapper
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    {
        protected IDbConnection connection;
        private CommandType commandType;

        protected GenericRepository(IDbConnection connection)
        {
            this.connection = connection;
            commandType = CommandType.Text;
        }

        public IEnumerable<TEntity> ExecuteQuery(string sqlText, object parameters)
        {
            try
            {
                commandType = CommandType.Text;

                if(parameters != null)
                {
                    return connection.Query<TEntity>(sqlText, parameters, commandType: commandType);
                }
                else
                {
                    return connection.Query<TEntity>(sqlText, commandType: commandType);
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<dynamic> ExecuteStoredProcedure(string nmStoredProcedure, object parameters)
        {
            try
            {
                commandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    return connection.Query(nmStoredProcedure, parameters, commandType: commandType);
                }
                else
                {
                    return connection.Query(nmStoredProcedure, commandType: commandType);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
      
    }
}