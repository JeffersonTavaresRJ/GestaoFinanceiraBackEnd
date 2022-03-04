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
        private MovimentacaoRealizadaDTO movimentacaoRealizadaDTO;

        public MovimentacaoRealizadaHandler(IMapper mapper, IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching)
        {
            this.mapper = mapper;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;            
        }

        public Task Handle(MovimentacaoRealizadaNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        foreach (MovimentacaoRealizada movimentacaoRealizada in notification.MovimentacoesRealizadas)
                        {
                            movimentacaoRealizadaDTO = mapper.Map<MovimentacaoRealizadaDTO>(movimentacaoRealizada);
                            movimentacaoRealizadaCaching.Add(movimentacaoRealizadaDTO);
                        }
                        break;
                    case ActionNotification.Atualizar:
                        movimentacaoRealizadaDTO = mapper.Map<MovimentacaoRealizadaDTO>(notification.MovimentacaoRealizada);
                        movimentacaoRealizadaCaching.Update(movimentacaoRealizadaDTO);
                        break;
                    case ActionNotification.Excluir:
                        movimentacaoRealizadaDTO = mapper.Map<MovimentacaoRealizadaDTO>(notification.MovimentacaoRealizada);
                        movimentacaoRealizadaCaching.Delete(movimentacaoRealizadaDTO);
                        break;
                }
            });
        }      
        
    }
}