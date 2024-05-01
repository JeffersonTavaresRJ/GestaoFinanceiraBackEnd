using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{
    public class ContaRequestHandler : IRequestHandler<CreateContaCommand>,
                                       IRequestHandler<UpdateContaCommand>,
                                       IRequestHandler<DeleteContaCommand>,
                                       IDisposable
    {

        private readonly IContaDomainService contaDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ContaRequestHandler(IContaDomainService contaDomainService, IMediator mediator, IMapper mapper)
        {
            this.contaDomainService = contaDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateContaCommand request, CancellationToken cancellationToken)
        {
            var conta = mapper.Map<Conta>(request);
            var validation = new ContaValidation().Validate(conta);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }
            contaDomainService.Add(conta);

            await mediator.Publish(new ContaNotification
            {
                Conta = conta,
                Action = ActionNotification.Criar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateContaCommand request, CancellationToken cancellationToken)
        {
            var conta = mapper.Map<Conta>(request);
            var validation = new ContaValidation().Validate(conta);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }
            contaDomainService.Update(conta);

            await mediator.Publish(new ContaNotification
            {
                Conta = conta,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteContaCommand request, CancellationToken cancellationToken)
        {
            var conta = mapper.Map<Conta>(request);
            contaDomainService.Delete(conta);

            await mediator.Publish(new ContaNotification
            {
                Conta = conta,
                Action = ActionNotification.Excluir
            });

            return Unit.Value;
        }

        public void Dispose()
        {
            contaDomainService.Dispose();
        }
    }
}