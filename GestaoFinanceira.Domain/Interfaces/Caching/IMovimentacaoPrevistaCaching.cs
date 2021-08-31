using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IMovimentacaoPrevistaCaching : IGenericWriteCaching<MovimentacaoPrevistaDTO>
    {
        MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoPrevistaDTO> GetByDataVencimento(int? idItemMovimentacao, int idUsuario, DateTime dataVencIni, DateTime dataVencFim);
    }
}
