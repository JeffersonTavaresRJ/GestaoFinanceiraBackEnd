using MediatR;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class DeleteMovimentacaoRealizadaCommand : IRequest
    {
        public int Id { get; set; }
    }
}
