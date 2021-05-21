using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IItemMovimentacaoApplicationService
    {
        Task Add(CreateItemMovimentacaoCommand command);
        Task Update(UpdateItemMovimentacaoCommand command);
        Task Delete(DeleteItemMovimentacaoCommand command);
        ItemMovimentacaoDTO GetId(int id);
        List<ItemMovimentacaoDTO> GetAll(int idUsuario);
        IList GetAllTipo();
    }
}
