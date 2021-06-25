using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IMovimentacaoPrevistaRepository : IGenericRepository<MovimentacaoPrevista>
    {
        MovimentacaoPrevista GetByKey(int idItemMovimentacao, DateTime idDataReferencia);
        IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idtemMovimentacao, DateTime dataRefIni, DateTime dataRefFim);
    }
}
