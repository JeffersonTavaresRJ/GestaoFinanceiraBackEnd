using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.Categoria;
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
    public class CategoriaRequestHandler : IRequestHandler<CreateCategoriaCommand>,
                                      IRequestHandler<UpdateCategoriaCommand>,
                                      IRequestHandler<DeleteCategoriaCommand>,
                                      IDisposable
    {
        private readonly ICategoriaDomainService categoriaDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CategoriaRequestHandler(ICategoriaDomainService categoriaDomainService, IMediator mediator, IMapper mapper)
        {
            this.categoriaDomainService = categoriaDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
        {
            //var categoria = new Categoria
            //{
            //    Descricao = request.Descricao,
            //    Tipo = (TipoCategoria)Enum.Parse(typeof(TipoCategoria), request.Tipo),
            //    IdUsuario = int.Parse(request.IdUsuario),
            //    Status = true,

            //};

            var categoria = mapper.Map<Categoria>(request);

            var validation = new CategoriaValidation().Validate(categoria);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            //base relacional..
            categoriaDomainService.Add(categoria);

            //base não relacional..
            await mediator.Publish(new CategoriaNotification
            {
                Categoria = categoria,
                Action = ActionNotification.Criar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
        {
            //var categoria = categoriaDomainService.GetId(int.Parse(request.Id));
            //Categoria.Descricao = request.Descricao;
            //Categoria.Tipo = (TipoCategoria)Enum.Parse(typeof(TipoCategoria), request.Tipo);
            //Categoria.Status = bool.Parse(request.Status);

            var categoria = mapper.Map<Categoria>(request);

            var validation = new CategoriaValidation().Validate(categoria);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            //base relacional..
            categoriaDomainService.Update(categoria);

            //base não relacional..
            await mediator.Publish(new CategoriaNotification
            {
                Categoria = categoria,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            //var categoria = categoriaDomainService.GetId(int.Parse(request.Id));
            var categoria = mapper.Map<Categoria>(request);

            //base relacional..
            categoriaDomainService.Delete(categoria);

            //base não relacional..
            await mediator.Publish(new CategoriaNotification
            {
                Categoria = categoria,
                Action = ActionNotification.Excluir
            });

            return Unit.Value;
        }

        public void Dispose()
        {
            categoriaDomainService.Dispose();
        }
    }
}