using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.FormaPagamento;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{
    public class FormaPagamentoRequestHandler : IRequestHandler<CreateFormaPagamentoCommand>,
                                                IRequestHandler<UpdateFormaPagamentoCommand>,
                                                IRequestHandler<DeleteFormaPagamentoCommand>
    {
        private readonly IFormaPagamentoDomainService formaPagamentoDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

       public FormaPagamentoRequestHandler(IFormaPagamentoDomainService formaPagamentoDomainService, IMediator mediator, IMapper mapper)
        {
            this.formaPagamentoDomainService = formaPagamentoDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateFormaPagamentoCommand request, CancellationToken cancellationToken)
        {
            var formaPagamento = mapper.Map<FormaPagamento>(request);

            var validate = new FormaPagamentoValidation().Validate(formaPagamento);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }

            formaPagamentoDomainService.Add(formaPagamento);

            await mediator.Publish(new FormaPagamentoNotification
            {
                Action = ActionNotification.Criar,
                FormaPagamento = formaPagamento
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateFormaPagamentoCommand request, CancellationToken cancellationToken)
        {
            var formaPagamento = mapper.Map<FormaPagamento>(request);

            var validate = new FormaPagamentoValidation().Validate(formaPagamento);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }

            formaPagamentoDomainService.Update(formaPagamento);

            await mediator.Publish(new FormaPagamentoNotification
            {
                Action = ActionNotification.Atualizar,
                FormaPagamento = formaPagamento
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteFormaPagamentoCommand request, CancellationToken cancellationToken)
        {
            var formaPagamento = mapper.Map<FormaPagamento>(request);                       

            formaPagamentoDomainService.Delete(formaPagamento);

            await mediator.Publish(new FormaPagamentoNotification
            {
                Action = ActionNotification.Excluir,
                FormaPagamento = formaPagamento
            });

            return Unit.Value;
        }
    }
}
