using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Dapper.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        public IEnumerable<TEntity> Execute( string sqlText, object parameters, TipoExecucao? tipo);
    }
}
