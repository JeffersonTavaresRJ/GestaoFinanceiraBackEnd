using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using AutoMapper;
using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using System.Threading.Tasks;
using System.Threading;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using FluentValidation;
using GestaoFinanceira.Application.Notifications;

namespace GestaoFinanceira.Application.RequestHandler
{
    public class ItemMovimentacaoRequestHandler : IRequestHandler<CreateItemMovimentacaoCommand>,
                                                  IRequestHandler<UpdateItemMovimentacaoCommand>,
                                                  IRequestHandler<DeleteItemMovimentacaoCommand>
    {
        private readonly IMapper mapper;
        private readonly IItemMovimentacaoDomainService itemMovimentacaoDomainService;
        private readonly IMediator mediator;

        public ItemMovimentacaoRequestHandler(IMapper mapper, IItemMovimentacaoDomainService itemMovimentacaoDomainService, IMediator mediator)
        {
            this.mapper = mapper;
            this.itemMovimentacaoDomainService = itemMovimentacaoDomainService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(CreateItemMovimentacaoCommand request, CancellationToken cancellationToken)
        {
            ItemMovimentacao itemMovimentacao = mapper.Map<ItemMovimentacao>(request);

            var validate = new ItemMovimentacaoValidation().Validate(itemMovimentacao);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }

            itemMovimentacaoDomainService.Add(itemMovimentacao);

            await mediator.Publish(new ItemMovimentacaoNotification
            {
                Action = ActionNotification.Criar,
                ItemMovimentacao = itemMovimentacao
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteItemMovimentacaoCommand request, CancellationToken cancellationToken)
        {
            var itemMovimentacao = mapper.Map<ItemMovimentacao>(request);

            itemMovimentacaoDomainService.Delete(itemMovimentacao);

            await mediator.Publish(new ItemMovimentacaoNotification
            {
                Action = ActionNotification.Excluir,
                ItemMovimentacao = itemMovimentacao
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateItemMovimentacaoCommand request, CancellationToken cancellationToken)
        {
            var itemMovimentacao = mapper.Map<ItemMovimentacao>(request);

            var validate = new ItemMovimentacaoValidation().Validate(itemMovimentacao);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }

            itemMovimentacaoDomainService.Update(itemMovimentacao);

            await mediator.Publish(new ItemMovimentacaoNotification
            {
                Action = ActionNotification.Atualizar,
                ItemMovimentacao = itemMovimentacao
            });

            return Unit.Value;
        }
    }
}
