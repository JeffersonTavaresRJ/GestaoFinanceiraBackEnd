using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestaoFinanceira.Domain.Models.Enuns;
using System.Collections;

namespace GestaoFinanceira.Application.Services
{
    public class CategoriaApplicationService : ICategoriaApplicationService
    {
        private readonly IMediator mediator;
        private readonly ICategoriaCaching categoriaCaching;

        public CategoriaApplicationService(IMediator mediator, ICategoriaCaching categoriaCaching)
        {
            this.mediator = mediator;
            this.categoriaCaching = categoriaCaching;
        }

        public async Task Add(CreateCategoriaCommand command)
        {
            await mediator.Send(command);
        }

        public async Task Update(UpdateCategoriaCommand command)
        {
            await mediator.Send(command);
        }

        public async Task Delete(DeleteCategoriaCommand command)
        {
            await mediator.Send(command);
        }

        public CategoriaDTO GetId(int id)
        {
            return categoriaCaching.GetId(id);
        }

        public List<CategoriaDTO> GetAll()
        {
            return categoriaCaching.GetAll();
        }
        
        public IList GetAllTipo()
        {
            return ExtensionEnum.Listar(typeof(TipoItemMovimentacao));
        }
    }
}
