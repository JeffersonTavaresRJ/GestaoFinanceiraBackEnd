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

        public MovimentacaoRealizadaHandler(IMapper mapper, IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching)
        {
            this.mapper = mapper;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
        }

        public Task Handle(MovimentacaoRealizadaNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                foreach (MovimentacaoRealizada movimentacaoRealizada in notification.MovimentacoesRealizadas)
                {
                    var movimentacaoRealizadaDTO = mapper.Map<MovimentacaoRealizadaDTO>(movimentacaoRealizada);

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
                }

            });
        }
    }
}