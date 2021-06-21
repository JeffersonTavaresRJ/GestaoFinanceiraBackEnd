using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models.Enuns;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class ItemMovimentacaoApplicationService : IItemMovimentacaoApplicationService
    {
        private readonly IMediator mediator;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;

        public ItemMovimentacaoApplicationService(IMediator mediator, IItemMovimentacaoCaching itemMovimentacaoCaching)
        {
            this.mediator = mediator;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
        }

        public async Task Add(CreateItemMovimentacaoCommand command)
        {
            await mediator.Send(command);
        }

        public async Task Update(UpdateItemMovimentacaoCommand command)
        {
            await mediator.Send(command);
        }

        public async Task Delete(DeleteItemMovimentacaoCommand command)
        {
            await mediator.Send(command);
        }

        public ItemMovimentacaoDTO GetId(int id)
        {
            return itemMovimentacaoCaching.GetId(id);
        }

        public List<ItemMovimentacaoDTO> GetAll(int idUsuario)
        {
            return  itemMovimentacaoCaching.GetAll(idUsuario);
        }

        public IList GetAllTipo()
        {
            return ExtensionEnum.Listar(typeof(TipoItemMovimentacao));
        }
    }
}
