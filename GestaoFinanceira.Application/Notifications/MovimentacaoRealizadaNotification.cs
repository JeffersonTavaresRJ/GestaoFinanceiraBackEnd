using GestaoFinanceira.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace GestaoFinanceira.Application.Notifications
{
    public class MovimentacaoRealizadaNotification : INotification
    {
        public List<MovimentacaoRealizada> MovimentacoesRealizadas { get; set; }
        public ActionNotification Action { get; set; }
    }
}
