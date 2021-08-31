using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
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


    public class MovimentacaoRealizadaRequestHandler : IRequestHandler<CreateMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<UpdateMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<DeleteMovimentacaoRealizadaCommand>
    {
        private readonly IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService;
        private readonly IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public MovimentacaoRealizadaRequestHandler(IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService, IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService, IMediator mediator, IMapper mapper)
        {
            this.movimentacaoRealizadaDomainService = movimentacaoRealizadaDomainService;
            this.movimentacaoPrevistaDomainService = movimentacaoPrevistaDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            List<MovimentacaoRealizada> movimentacoesRealizadas = new List<MovimentacaoRealizada>();
            foreach (MovimentacaoRealizadaCommand item in request.MovimentacaoRealizadaCommand)
            {
                MovimentacaoRealizada movimentacaoRealizada = mapper.Map<MovimentacaoRealizada>(item);

                var validate = new MovimentacaoRealizadaValidation().Validate(movimentacaoRealizada);
                if (!validate.IsValid)
                {
                    throw new ValidationException(validate.Errors);
                }

                movimentacoesRealizadas.Add(movimentacaoRealizada);

            }
            movimentacaoRealizadaDomainService.Add(movimentacoesRealizadas);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Criar
            });

            MovimentacaoPrevista movimentacaoPrevista = movimentacaoPrevistaDomainService
                                               .GetByKey(movimentacoesRealizadas[0].IdItemMovimentacao,
                                                         movimentacoesRealizadas[0].DataReferencia);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Atualizar
            });
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoRealizada movimentacaoRealizada = mapper.Map<MovimentacaoRealizada>(request);

            var validate = new MovimentacaoRealizadaValidation().Validate(movimentacaoRealizada);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            movimentacaoRealizadaDomainService.Update(movimentacaoRealizada);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacaoRealizada = movimentacaoRealizada,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoRealizada movimentacaoRealizada = movimentacaoRealizadaDomainService.GetId(request.Id);
            movimentacaoRealizadaDomainService.Delete(movimentacaoRealizada);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacaoRealizada = movimentacaoRealizada,
                Action = ActionNotification.Excluir
            });

            MovimentacaoPrevista movimentacaoPrevista = movimentacaoPrevistaDomainService.GetByKey(movimentacaoRealizada.IdItemMovimentacao,
                                                                                                       movimentacaoRealizada.DataReferencia);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Atualizar
            });
            return Unit.Value;
        }
    }
}