using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IMovimentacaoPrevistaCaching : IGenericWriteCaching<MovimentacaoPrevistaDTO>
    {
        MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoPrevistaDTO> GetAll();
        List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime? dataVencIni, DateTime? dataVencFim, int? idItemMovimentacao);
    }
}
