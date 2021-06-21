using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
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
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public MovimentacaoPrevistaRequestHandler(IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService, IMediator mediator, IMapper mapper)
        {
            this.movimentacaoPrevistaDomainService = movimentacaoPrevistaDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public Task<Unit> Handle(CreateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);
            movimentacaoPrevista.Movimentacao = mapper.Map<Movimentacao>(request);
            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            movimentacaoPrevistaDomainService.Add(movimentacaoPrevista, request.QtdeParcelas);        
            return Unit.Task;
        }

        public Task<Unit> Handle(UpdateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);
            movimentacaoPrevista.Movimentacao = mapper.Map<Movimentacao>(request);
            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            movimentacaoPrevistaDomainService.Update(movimentacaoPrevista);
            return Unit.Task;
        }

        public Task<Unit> Handle(DeleteMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);
            movimentacaoPrevistaDomainService.Delete(movimentacaoPrevista);
            return Unit.Task;
        }
    }
}
