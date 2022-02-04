using GestaoFinanceira.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace GestaoFinanceira.Application.Notifications
{
    public class SaldoDiarioNotification : INotification
    {
        public List<SaldoDiario> SaldosDiario { get; set; }
        public ActionNotification Action { get; set; }
    }
}
