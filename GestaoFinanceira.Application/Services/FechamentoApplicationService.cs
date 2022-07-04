using GestaoFinanceira.Application.Commands.Fechamento;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class FechamentoApplicationService : IFechamentoApplicationService
    {
        private readonly IMediator mediator;

        public FechamentoApplicationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Executar(CreateFechamentoCommand fechamentoCreateCommand)
        {
            await mediator.Send(fechamentoCreateCommand);
        }
    }
}
