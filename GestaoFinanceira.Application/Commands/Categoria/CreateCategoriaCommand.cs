using MediatR;

namespace GestaoFinanceira.Application.Commands.Categoria
{
    public class CreateCategoriaCommand :IRequest
    {
        public string Descricao { get; set; }
    }
}
