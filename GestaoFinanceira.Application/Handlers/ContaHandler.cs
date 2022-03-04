using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class ContaHandler : INotificationHandler<ContaNotification>
    {
        private readonly IContaCaching contaCaching;
        private readonly IMapper mapper;

        public ContaHandler(IContaCaching contaCaching, IMapper mapper)
        {
            this.contaCaching = contaCaching;
            this.mapper = mapper;
        }

        public Task Handle(ContaNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var contaDTO = mapper.Map<ContaDTO>(notification.Conta);

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        contaCaching.Add(contaDTO);
                        break;
                    case ActionNotification.Atualizar:
                        contaCaching.Update(contaDTO);
                        break;
                    case ActionNotification.Excluir:
                        contaCaching.Delete(contaDTO);
                        break;
                    default:
                        break;
                }

            });
        }
    }
}
