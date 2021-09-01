using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
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
        private readonly IMovimentacaoDomainService movimentacaoDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private MovimentacaoPrevista movimentacaoPrevista;

        public MovimentacaoRealizadaRequestHandler(IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService, 
                                                   IMovimentacaoDomainService movimentacaoDomainService, 
                                                   IMediator mediator, 
                                                   IMapper mapper)
        {
            this.movimentacaoRealizadaDomainService = movimentacaoRealizadaDomainService;
            this.movimentacaoDomainService = movimentacaoDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            List<MovimentacaoRealizada> movimentacoesRealizadas = new List<MovimentacaoRealizada>();
            foreach (MovimentacaoRealizadaCommand item in request.MovimentacaoRealizadaCommand)
            {
                MovimentacaoRealizada movimentacaoRealizada = mapper.Map<MovimentacaoRealizada>(item);
                //movimentacaoRealizada.Movimentacao = movimentacaoDomainService.GetByKey(item.IdItemMovimentacao, item.DataReferencia);

                var validate = new MovimentacaoRealizadaValidation().Validate(movimentacaoRealizada);
                if (!validate.IsValid)
                {
                    throw new ValidationException(validate.Errors);
                }

                movimentacoesRealizadas.Add(movimentacaoRealizada);

            }
            movimentacaoRealizadaDomainService.Add(movimentacoesRealizadas, out movimentacaoPrevista);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Criar
            });

            if(movimentacaoPrevista!= null)
            {
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacaoPrevista = movimentacaoPrevista,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status);
            }
            
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
            movimentacaoRealizadaDomainService.Update(movimentacaoRealizada, out movimentacaoPrevista);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacaoRealizada = movimentacaoRealizada,
                Action = ActionNotification.Atualizar
            });

            if (movimentacaoPrevista != null)
            {
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacaoPrevista = movimentacaoPrevista,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoRealizada movimentacaoRealizada = movimentacaoRealizadaDomainService.GetId(request.Id);
            movimentacaoRealizadaDomainService.Delete(movimentacaoRealizada, out movimentacaoPrevista);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacaoRealizada = movimentacaoRealizada,
                Action = ActionNotification.Excluir
            });

            if (movimentacaoPrevista != null)
            {
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacaoPrevista = movimentacaoPrevista,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status);
            }
            return Unit.Value;
        }
    }
}