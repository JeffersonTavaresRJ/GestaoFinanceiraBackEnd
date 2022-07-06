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
        private readonly IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService;
        private readonly IMediator mediator;

        public FechamentoRequestHandler(IFechamentoDomainService fechamentoDomainService, IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService, IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService, IMediator mediator)
        {
            this.fechamentoDomainService = fechamentoDomainService;
            this.movimentacaoPrevistaDomainService = movimentacaoPrevistaDomainService;
            this.movimentacaoRealizadaDomainService = movimentacaoRealizadaDomainService;
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

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = movimentacoesPrevistas,
                Action = ActionNotification.Atualizar
            });

            List<MovimentacaoRealizada> movimentacoesRealizadas =
                movimentacaoRealizadaDomainService.GetByUsuario(request.IdUsuario, dataFim);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Atualizar
            });
            return Unit.Value;
        }
    }
}
