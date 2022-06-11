using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class MovimentacaoRealizadaHandler : INotificationHandler<MovimentacaoRealizadaNotification>
    {
        private readonly IMapper mapper;
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly IContaCaching contaCaching;
        private readonly IFormaPagamentoCaching formaPagamentoCaching;
        private MovimentacaoRealizadaDTO movimentacaoRealizadaDTO;

        public MovimentacaoRealizadaHandler(IMapper mapper, IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching, IContaCaching contaCaching, IFormaPagamentoCaching formaPagamentoCaching)
        {
            this.mapper = mapper;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
            this.contaCaching = contaCaching;
            this.formaPagamentoCaching = formaPagamentoCaching;
        }

        public Task Handle(MovimentacaoRealizadaNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                movimentacaoRealizadaDTO = mapper.Map<MovimentacaoRealizadaDTO>(notification.MovimentacaoRealizada);

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        movimentacaoRealizadaCaching.Add(movimentacaoRealizadaDTO);
                        break;
                    case ActionNotification.Atualizar:
                        movimentacaoRealizadaCaching.Update(movimentacaoRealizadaDTO);
                        break;
                    case ActionNotification.Excluir:
                        movimentacaoRealizadaCaching.Delete(movimentacaoRealizadaDTO);
                        break;
                }
            });
        }
    }
}