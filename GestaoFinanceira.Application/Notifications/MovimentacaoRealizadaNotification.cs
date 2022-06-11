using GestaoFinanceira.Domain.Models;
using MediatR;

namespace GestaoFinanceira.Application.Notifications
{
    public class MovimentacaoRealizadaNotification : INotification
    {
        public MovimentacaoRealizada MovimentacaoRealizada { get; set; }
        public ActionNotification Action { get; set; }
    }
}
