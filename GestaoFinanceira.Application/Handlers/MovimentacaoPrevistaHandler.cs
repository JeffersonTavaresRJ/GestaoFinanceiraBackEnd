using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class MovimentacaoPrevistaHandler : INotificationHandler<MovimentacaoPrevistaNotification>
    {
        private readonly IMapper mapper;
        private readonly IMovimentacaoPrevistaCaching movimentacaoPrevistaCaching;
        private readonly IFormaPagamentoCaching formaPagamentoCaching;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;
        private MovimentacaoPrevistaDTO movimentacaoPrevistaDTO;

        public MovimentacaoPrevistaHandler(IMapper mapper, IMovimentacaoPrevistaCaching movimentacaoPrevistaCaching, 
            IFormaPagamentoCaching formaPagamentoCaching, IItemMovimentacaoCaching itemMovimentacaoCaching)
        {
            this.mapper = mapper;
            this.movimentacaoPrevistaCaching = movimentacaoPrevistaCaching;
            this.formaPagamentoCaching = formaPagamentoCaching;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
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
                            movimentacaoPrevistaDTO = Convert(movimentacaoPrevista);
                            movimentacaoPrevistaCaching.Add(movimentacaoPrevistaDTO);
                        }                                                
                        break;
                    case ActionNotification.Atualizar:
                        movimentacaoPrevistaDTO = Convert(notification.MovimentacaoPrevista);
                        movimentacaoPrevistaCaching.Update(movimentacaoPrevistaDTO);
                        break;
                    case ActionNotification.Excluir:
                        movimentacaoPrevistaDTO = Convert(notification.MovimentacaoPrevista);
                        movimentacaoPrevistaCaching.Delete(movimentacaoPrevistaDTO);
                        break;
                }
            });
        }

        private MovimentacaoPrevistaDTO Convert(MovimentacaoPrevista movimentacaoPrevista)
        {
            MovimentacaoPrevistaDTO movimentacaoPrevistaDTO = mapper.Map<MovimentacaoPrevistaDTO>(movimentacaoPrevista);
            movimentacaoPrevistaDTO.FormaPagamento = formaPagamentoCaching.GetId(movimentacaoPrevista.IdFormaPagamento);
            movimentacaoPrevistaDTO.ItemMovimentacao = itemMovimentacaoCaching.GetId(movimentacaoPrevista.IdItemMovimentacao);  
            return movimentacaoPrevistaDTO;
        }
    }
}