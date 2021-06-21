using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class MovimentacaoPrevistaApplicationService : IMovimentacaoPrevistaApplicationService
    {

        private readonly IMediator mediator;

        public MovimentacaoPrevistaApplicationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task Add(CreateMovimentacaoPrevistaCommand command)
        {
            return mediator.Send(command);
        }

        public Task Update(UpdateMovimentacaoPrevistaCommand command)
        {
            return mediator.Send(command);
        }

        public Task Delete(DeleteMovimentacaoPrevistaCommand command)
        {
            return mediator.Send(command);
        }
    }
}
