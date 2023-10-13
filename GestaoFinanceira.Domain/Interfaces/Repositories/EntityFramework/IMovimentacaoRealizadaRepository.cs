using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IMovimentacaoRealizadaRepository : IGenericRepository<MovimentacaoRealizada>
    {
        IEnumerable<MovimentacaoRealizada> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia);
        IEnumerable<MovimentacaoRealizada> GetByUsuario(int idUsuario, DateTime dataReferencia);
    }
}
