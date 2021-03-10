using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models.Enuns;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class CategoriaHandler : INotificationHandler<CategoriaNotification>
    {
        private readonly ICategoriaCaching categoriaCaching;
        private readonly IMapper mapper;

        public CategoriaHandler(ICategoriaCaching categoriaCaching, IMapper mapper)
        {
            this.categoriaCaching = categoriaCaching;
            this.mapper = mapper;
        }

        public Task Handle(CategoriaNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                //var categoriaDTO = new CategoriaDTO
                //{
                //    Id = notification.Categoria.Id,
                //    Descricao = notification.Categoria.Descricao,
                //    Tipo = ExtensionEnum.ObterDescricao((TipoCategoria)Enum.Parse(typeof(TipoCategoria), notification.Categoria.Tipo.ToString())),
                //    IdUsuario = notification.Categoria.IdUsuario,
                //    Status = notification.Categoria.Status

                //};

                var categoriaDTO = mapper.Map<CategoriaDTO>(notification.Categoria);

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        categoriaCaching.Add(categoriaDTO);
                        break;
                    case ActionNotification.Atualizar:
                        categoriaCaching.Update(categoriaDTO);
                        break;
                    case ActionNotification.Excluir:
                        categoriaCaching.Delete(categoriaDTO);
                        break;
                }
            });
        }
    }
}
