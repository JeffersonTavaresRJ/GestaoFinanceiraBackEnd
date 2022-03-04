using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class FormaPagamentoHandler: INotificationHandler<FormaPagamentoNotification>
    {
        private readonly IFormaPagamentoCaching formaPagamentoCaching;
        private readonly IMapper mapper;

        public FormaPagamentoHandler(IFormaPagamentoCaching formaPagamentoCaching, IMapper mapper)
        {
            this.formaPagamentoCaching = formaPagamentoCaching;
            this.mapper = mapper;
        }

        public Task Handle(FormaPagamentoNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var formaPagamentoDTO = mapper.Map<FormaPagamentoDTO>(notification.FormaPagamento);

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        formaPagamentoCaching.Add(formaPagamentoDTO);
                        break;
                    case ActionNotification.Atualizar:
                        formaPagamentoCaching.Update(formaPagamentoDTO);
                        break;
                    case ActionNotification.Excluir:
                        formaPagamentoCaching.Delete(formaPagamentoDTO);
                        break;
                }
            });
        }
    }
}
