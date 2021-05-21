using MediatR;

namespace GestaoFinanceira.Application.Commands.ItemMovimentacao
{
    public class CreateItemMovimentacaoCommand : IRequest
    {
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int IdCategoria { get; set; }
    }
}
