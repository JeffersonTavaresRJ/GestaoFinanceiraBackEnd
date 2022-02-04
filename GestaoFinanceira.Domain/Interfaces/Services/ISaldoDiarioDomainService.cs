using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface ISaldoDiarioDomainService
    {
        List<SaldoDiario> GetBySaldosDiario(int idConta, DateTime dataSaldo);
        SaldoDiario GetByKey(int idConta, DateTime dataSaldo);
    }
}
