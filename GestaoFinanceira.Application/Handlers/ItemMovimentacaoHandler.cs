using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class ItemMovimentacaoHandler : INotificationHandler<ItemMovimentacaoNotification>
    {
        private readonly IMapper mapper;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;
        public ItemMovimentacaoHandler(IMapper mapper, IItemMovimentacaoCaching itemMovimentacaoCaching)
        {
            this.mapper = mapper;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
        }

        public Task Handle(ItemMovimentacaoNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(()=>
            {
                ItemMovimentacaoDTO itemMovimentacaoDTO = mapper.Map<ItemMovimentacaoDTO>(notification.ItemMovimentacao);
                itemMovimentacaoDTO.Categoria.IdUsuario = notification.IdUsuario;

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
