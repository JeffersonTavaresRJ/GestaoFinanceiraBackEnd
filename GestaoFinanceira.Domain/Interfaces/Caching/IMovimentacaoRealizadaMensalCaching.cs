using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IMovimentacaoRealizadaMensalCaching
    {
        List<MovimentacaoRealizadaMensalDTO> GetByMovimentacaoRealizadaMensal(List<int> idsConta, DateTime dataReferencia, int totalmeses);
    }
}
