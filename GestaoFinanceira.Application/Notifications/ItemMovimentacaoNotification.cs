using System;
using System.Collections.Generic;
using System.Text;
using GestaoFinanceira.Domain.Models;
using MediatR;

namespace GestaoFinanceira.Application.Notifications
{
    public class ItemMovimentacaoNotification : INotification
    {
        public ActionNotification Action { get; set; }
        public ItemMovimentacao ItemMovimentacao { get; set; }
        public int IdUsuario { get; set; }
    }
}
