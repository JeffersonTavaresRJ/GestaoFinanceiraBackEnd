using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IMovimentacaoPrevistaCaching : IGenericWriteCaching<MovimentacaoPrevistaDTO>
    {
        MovimentacaoPrevistaDTO GetId(int id);
        List<MovimentacaoPrevistaDTO> GetAll();
        List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime? dataVencIni, DateTime? dataVencFim, int? idItemMovimentacao);
    }
}
