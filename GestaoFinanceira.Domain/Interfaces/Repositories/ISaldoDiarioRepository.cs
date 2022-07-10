using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface ISaldoDiarioRepository
    {
        IEnumerable<SaldoDiario> GetBySaldosDiario(int idConta, DateTime dataSaldo);
        IEnumerable<SaldoDiario> GetByPeriodo(int idUsuario, DateTime dataIni, DateTime dataFim);
        SaldoDiario GetByKey(int idConta, DateTime dataSaldo);
    }
}
