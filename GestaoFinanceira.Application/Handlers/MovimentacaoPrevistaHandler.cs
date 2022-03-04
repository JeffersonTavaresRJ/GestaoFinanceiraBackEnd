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
    public class MovimentacaoPrevistaHandler : INotificationHandler<MovimentacaoPrevistaNotification>
    {
        private readonly IMapper mapper;
        private readonly IMovimentacaoPrevistaCaching movimentacaoPrevistaCaching;
        private MovimentacaoPrevistaDTO movimentacaoPrevistaDTO;

        public MovimentacaoPrevistaHandler(IMapper mapper, IMovimentacaoPrevistaCaching movimentacaoPrevistaCaching)
        {
            this.mapper = mapper;
            this.movimentacaoPrevistaCaching = movimentacaoPrevistaCaching;           
        }

        public Task Handle(MovimentacaoPrevistaNotification notification, CancellationToken cancellationToken)
        {
            
            return Task.Run(() =>
            {
                
                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        foreach (MovimentacaoPrevista movimentacaoPrevista in notification.MovimentacoesPrevistas)
                        {
                            movimentacaoPrevistaDTO = Convert(movimentacaoPrevista, ActionNotification.Criar);
                            movimentacaoPrevistaCaching.Add(movimentacaoPrevistaDTO);
                        }                                                
                        break;
                    case ActionNotification.Atualizar:
                        movimentacaoPrevistaDTO = Convert(notification.MovimentacaoPrevista, ActionNotification.Atualizar);
                        movimentacaoPrevistaCaching.Update(movimentacaoPrevistaDTO);
                        break;
                    case ActionNotification.Excluir:
                        movimentacaoPrevistaDTO = Convert(notification.MovimentacaoPrevista, ActionNotification.Excluir);
                        movimentacaoPrevistaCaching.Delete(movimentacaoPrevistaDTO);
                        break;
                }
            });
        }

        private MovimentacaoPrevistaDTO Convert(MovimentacaoPrevista movimentacaoPrevista, ActionNotification action)
        {
            MovimentacaoPrevistaDTO movimentacaoPrevistaDTO = mapper.Map<MovimentacaoPrevistaDTO>(movimentacaoPrevista);          
                        
            if(action.Equals(ActionNotification.Atualizar) && movimentacaoPrevista.NrParcelaTotal <= 1)
            {
                movimentacaoPrevistaDTO.Parcela = movimentacaoPrevistaCaching.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                                       movimentacaoPrevista.DataReferencia).Parcela;
            }
            return movimentacaoPrevistaDTO;
        }
    }
}