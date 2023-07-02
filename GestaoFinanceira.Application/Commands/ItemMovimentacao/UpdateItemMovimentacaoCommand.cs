using MediatR;

namespace GestaoFinanceira.Application.Commands.ItemMovimentacao
{
    public class UpdateItemMovimentacaoCommand : IRequest
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public bool Status { get; set; }
        public int IdCategoria { get; set; }
        public string TipoOperacao { get; set; }

    }
}
