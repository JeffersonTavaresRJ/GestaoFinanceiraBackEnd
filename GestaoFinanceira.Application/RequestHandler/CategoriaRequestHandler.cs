using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using GestaoFinanceira.Domain.Validations;
using FluentValidation;
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

        public CategoriaRequestHandler(ICategoriaDomainService categoriaDomainService, IMediator mediator)
        {
            this.categoriaDomainService = categoriaDomainService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
        {
            var Categoria = new Categoria
            {
                Descricao = request.Descricao,
                Tipo = (TipoCategoria)Enum.Parse(typeof(TipoCategoria), request.Tipo),
                IdUsuario = int.Parse(request.IdUsuario),
                Status = true,

            };

            var validation = new CategoriaValidation().Validate(Categoria);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            //base relacional..
            categoriaDomainService.Add(Categoria);

            //base não relacional..
            await mediator.Publish(new CategoriaNotification
            {
                Categoria = Categoria,
                Action = ActionNotification.Criar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
        {
            var Categoria = categoriaDomainService.GetId(int.Parse(request.Id));
            Categoria.Descricao = request.Descricao;
            Categoria.Tipo = (TipoCategoria)Enum.Parse(typeof(TipoCategoria), request.Tipo);
            Categoria.Status = bool.Parse(request.Status);

            var validation = new CategoriaValidation().Validate(Categoria);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            //base relacional..
            categoriaDomainService.Update(Categoria);

            //base não relacional..
            await mediator.Publish(new CategoriaNotification
            {
                Categoria = Categoria,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            var Categoria = categoriaDomainService.GetId(int.Parse(request.Id));

            //base relacional..
            categoriaDomainService.Delete(Categoria);

            //base não relacional..
            await mediator.Publish(new CategoriaNotification
            {
                Categoria = Categoria,
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