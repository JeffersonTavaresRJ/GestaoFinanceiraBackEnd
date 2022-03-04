using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface ISaldoDiarioCaching : IGenericWriteCaching<SaldoDiarioDTO>
    {
        SaldoDiarioDTO GetByKey(int idConta, DateTime dataSaldo);
        List<SaldoDiarioDTO> GetAll();
        List<SaldoDiarioDTO> GetBySaldosDiario(int idConta, DateTime dataSaldo);
        List<SaldoDiarioDTO> GetGroupBySaldoDiario(DateTime dataIni, DateTime dataFim);
    }
}
