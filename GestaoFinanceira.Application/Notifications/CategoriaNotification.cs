using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Notifications
{
    public class CategoriaNotification : INotification
    {
        public Categoria Categoria { get; set; }
        public ActionNotification Action { get; set; }
    }
}
