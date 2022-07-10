using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface ISaldoDiarioDomainService
    {
        List<SaldoDiario> GetBySaldosDiario(int idConta, DateTime dataSaldo);
        List<SaldoDiario> GetByPeriodo(int idUsuario, DateTime dataIni, DateTime dataFim);
        SaldoDiario GetByKey(int idConta, DateTime dataSaldo);
    }
}
