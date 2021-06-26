using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
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
            if(request.QtdeParcelas <=0)
            {
                throw new TotalParcelasMovimentacaoInvalidoException(0);
            }
            
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);

            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            movimentacaoPrevistaDomainService.Add(movimentacaoPrevista, request.QtdeParcelas);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Criar,
                QtdeParcelas = request.QtdeParcelas
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
                Action = ActionNotification.Atualizar,
                QtdeParcelas = 0
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
                Action = ActionNotification.Excluir,
                QtdeParcelas = 0
            });
            

            return Unit.Value;
        }
    }
}