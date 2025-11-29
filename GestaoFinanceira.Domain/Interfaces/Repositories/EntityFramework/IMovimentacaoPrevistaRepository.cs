using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IMovimentacaoPrevistaRepository : IGenericRepository<MovimentacaoPrevista>
    {
        IEnumerable<MovimentacaoPrevista> GetByKey(int idItemMovimentacao, DateTime idDataReferencia);
        IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idtemMovimentacao, DateTime dataRefIni, DateTime dataRefFim);
        IEnumerable<MovimentacaoPrevista> GetByMovPrevParcelada(int idMovPrevParcelada);
    }
}
