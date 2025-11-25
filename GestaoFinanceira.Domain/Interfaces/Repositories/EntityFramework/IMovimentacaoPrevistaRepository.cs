using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IMovimentacaoPrevistaRepository : IGenericRepository<MovimentacaoPrevista>
    {
        MovimentacaoPrevista GetByKey(int idItemMovimentacao, DateTime idDataReferencia);
        IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idtemMovimentacao, DateTime dataRefIni, DateTime dataRefFim);
        IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idFormaPagamento, int idtemMovimentacao, DateTime dataRefIni, DateTime dataRefFim);
    }
}
