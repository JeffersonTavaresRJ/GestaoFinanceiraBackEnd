using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Dapper.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        public IEnumerable<dynamic> ExecuteStoredProcedure( string nmStoredProcedure, object parameters);
        public IEnumerable<TEntity> ExecuteQuery(string sqlText, object parameters);
    }
}
