using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GestaoFinanceira.Application.Notifications;
using MediatR;
using AutoMapper;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.DTOs;

namespace GestaoFinanceira.Application.Handlers
{
    public class ItemMovimentacaoHandler : INotificationHandler<ItemMovimentacaoNotification>
    {
        private readonly IMapper mapper;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;
        private readonly ICategoriaCaching categoriaCaching;

        public ItemMovimentacaoHandler(IMapper mapper, ICategoriaCaching categoriaCaching, IItemMovimentacaoCaching itemMovimentacaoCaching)
        {
            this.mapper = mapper;
            this.categoriaCaching = categoriaCaching;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
        }

        public Task Handle(ItemMovimentacaoNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(()=>
            {
                ItemMovimentacaoDTO itemMovimentacaoDTO = mapper.Map<ItemMovimentacaoDTO>(notification.ItemMovimentacao);
                itemMovimentacaoDTO.Categoria = categoriaCaching.GetId(notification.ItemMovimentacao.IdCategoria);

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        itemMovimentacaoCaching.Add(itemMovimentacaoDTO);
                        break;
                    case ActionNotification.Atualizar:
                        itemMovimentacaoCaching.Update(itemMovimentacaoDTO);
                        break;
                    case ActionNotification.Excluir:
                        itemMovimentacaoCaching.Delete(itemMovimentacaoDTO);
                        break;
                }
            });
        }
    }
}
