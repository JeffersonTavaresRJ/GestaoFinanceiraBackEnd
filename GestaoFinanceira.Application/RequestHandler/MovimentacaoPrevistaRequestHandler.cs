using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{
    public class MovimentacaoPrevistaRequestHandler : IRequestHandler<CreateMovimentacaoPrevistaCommand>,
                                                      IRequestHandler<UpdateMovimentacaoPrevistaCommand>,
                                                      IRequestHandler<DeleteMovimentacaoPrevistaCommand>
    {
        private readonly IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService;
        private readonly IMovimentacaoDomainService movimentacaoDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public MovimentacaoPrevistaRequestHandler(IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService,
                                                  IMovimentacaoDomainService movimentacaoDomainService,
                                                  IMediator mediator, 
                                                  IMapper mapper)
        {
            this.movimentacaoPrevistaDomainService = movimentacaoPrevistaDomainService;
            this.movimentacaoDomainService = movimentacaoDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);

            Movimentacao movimentacao = movimentacaoDomainService.GetByKey(movimentacaoPrevista.IdItemMovimentacao, movimentacaoPrevista.DataReferencia);

            if (movimentacao.MovimentacoesRealizadas.Count > 0 && movimentacaoPrevista.Status != Domain.Models.Enuns.StatusMovimentacaoPrevista.Q)
            {
                throw new StatusMovimentacaoInvalidoException(movimentacao.ItemMovimentacao.Descricao, 
                                                              movimentacao.DataReferencia);
            }

            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            movimentacaoPrevistaDomainService.Add(movimentacaoPrevista, request.QtdeParcelas);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Criar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);
       
            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            movimentacaoPrevistaDomainService.Update(movimentacaoPrevista);
            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = movimentacaoPrevistaDomainService.GetByKey(request.IdItemMovimentacao, request.DataReferencia);
;           movimentacaoPrevistaDomainService.Delete(movimentacaoPrevista);           
            
            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Excluir
            });
            

            return Unit.Value;
        }
    }
}