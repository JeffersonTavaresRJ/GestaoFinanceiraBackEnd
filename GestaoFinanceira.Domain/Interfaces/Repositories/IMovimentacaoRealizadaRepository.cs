using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IMovimentacaoRealizadaRepository : IGenericRepository<MovimentacaoRealizada>
    {
        IEnumerable<MovimentacaoRealizada> GetByDataReferencia(int? idItemMovimentacao, int idUsuario, DateTime dataRefIni, DateTime dataRefFim);
    }
}
