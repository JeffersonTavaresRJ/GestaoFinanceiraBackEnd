using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Notifications
{
    public class MovimentacaoPrevistaNotification : INotification
    {
        public MovimentacaoPrevista MovimentacaoPrevista { get; set; }
        public List<MovimentacaoPrevista> MovimentacoesPrevistas { get; set; }
        public ActionNotification Action { get; set; }

    }
}
