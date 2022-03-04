using GestaoFinanceira.Domain.Models;
using MediatR;

namespace GestaoFinanceira.Application.Notifications
{
    public class CategoriaNotification : INotification
    {
        public Categoria Categoria { get; set; }
        public ActionNotification Action { get; set; }
    }
}
