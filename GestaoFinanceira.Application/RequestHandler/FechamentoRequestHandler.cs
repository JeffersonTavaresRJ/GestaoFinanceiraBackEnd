using GestaoFinanceira.Application.Commands.Fechamento;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{
    public class FechamentoRequestHandler : IRequestHandler<CreateFechamentoCommand>
    {
        private readonly IFechamentoDomainService fechamentoDomainService;
        private readonly IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService;
        private readonly ISaldoDiarioDomainService saldoDiarioDomainService;
        private readonly IMediator mediator;

        public FechamentoRequestHandler(IFechamentoDomainService fechamentoDomainService, IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService, ISaldoDiarioDomainService saldoDiarioDomainService, IMediator mediator)
        {
            this.fechamentoDomainService = fechamentoDomainService;
            this.movimentacaoPrevistaDomainService = movimentacaoPrevistaDomainService;
            this.saldoDiarioDomainService = saldoDiarioDomainService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(CreateFechamentoCommand request, CancellationToken cancellationToken)
        {
            fechamentoDomainService.Executar(request.IdUsuario, request.DataReferencia);

            var dataIni = new DateTime(request.DataReferencia.Year,
                                       request.DataReferencia.Month,
                                       1);
            var dataFim = request.DataReferencia;

            List<MovimentacaoPrevista> movimentacoesPrevistas =
                movimentacaoPrevistaDomainService.GetByDataReferencia(request.IdUsuario, null, dataIni, dataFim);
            
            _ = mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = movimentacoesPrevistas,
                Action = ActionNotification.Atualizar
            });

            List<SaldoDiario> saldosDiario =
                saldoDiarioDomainService.GetByPeriodo(request.IdUsuario, dataIni, dataFim);

            await mediator.Publish(new SaldoDiarioNotification
            {
                SaldosDiario = saldosDiario,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }
    }
}
