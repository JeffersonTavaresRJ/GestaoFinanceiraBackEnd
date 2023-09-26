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
        List<SaldoDiarioDTO> GetBySaldosDiario(DateTime dataSaldoIni, DateTime dataSaldoFim);
        List<SaldoDiarioDTO> GetGroupBySaldoDiario(DateTime dataIni, DateTime dataFim);
        List<SaldoDiarioDTO> GetMaxGroupBySaldoConta(DateTime dataReferencia);
        double GetSaldoConta(int idConta, DateTime dataReferencia);
    }
}
