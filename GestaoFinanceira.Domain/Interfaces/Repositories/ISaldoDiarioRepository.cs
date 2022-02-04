using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface ISaldoDiarioRepository
    {
        IEnumerable<SaldoDiario> GetBySaldosDiario(int idConta, DateTime dataSaldo);
        SaldoDiario GetByKey(int idConta, DateTime dataSaldo);
    }
}
