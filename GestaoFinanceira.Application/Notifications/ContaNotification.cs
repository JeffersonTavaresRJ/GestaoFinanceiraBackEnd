using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Notifications
{
    public class ContaNotification : INotification
    {
        public Conta Conta { get; set; }
        public ActionNotification Action { get; set; }

    }
}
