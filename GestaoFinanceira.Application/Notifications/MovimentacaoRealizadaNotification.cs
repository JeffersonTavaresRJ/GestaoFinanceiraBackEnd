using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Notifications
{
    public class MovimentacaoRealizadaNotification : INotification
    {
        public MovimentacaoRealizada MovimentacaoRealizada { get; set; }
        public List<MovimentacaoRealizada> MovimentacoesRealizadas { get; set; }
        public ActionNotification Action { get; set; }
    }
}
