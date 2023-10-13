using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Dapper.Repositories
{
    public interface IGenericRepository
    {
        public IEnumerable<dynamic> Execute( string sqlText, object parameters, TipoExecucao? tipo);        
    }
}
