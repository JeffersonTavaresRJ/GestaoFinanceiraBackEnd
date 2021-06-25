using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
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
                MovimentacaoPrevistaDTO movimentacaoPrevistaDTO = mapper.Map<MovimentacaoPrevistaDTO>(notification.MovimentacaoPrevista);
                movimentacaoPrevistaDTO.FormaPagamento = formaPagamentoCaching.GetId(notification.MovimentacaoPrevista.IdFormaPagamento);
                movimentacaoPrevistaDTO.ItemMovimentacao = itemMovimentacaoCaching.GetId(notification.MovimentacaoPrevista.IdItemMovimentacao);

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        movimentacaoPrevistaCaching.Add(movimentacaoPrevistaDTO);
                        break;
                    case ActionNotification.Atualizar:
                        movimentacaoPrevistaCaching.Update(movimentacaoPrevistaDTO);
                        break;
                    case ActionNotification.Excluir:
                        movimentacaoPrevistaCaching.Delete(movimentacaoPrevistaDTO);
                        break;
                }
            });
        }
    }
}