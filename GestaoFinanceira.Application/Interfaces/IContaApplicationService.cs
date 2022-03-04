using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IContaApplicationService
    {
        Task Add(CreateContaCommand command);
        Task Update(UpdateContaCommand command);
        Task Delete(DeleteContaCommand command);
        ContaDTO GetId(int id);
        List<ContaDTO> GetAll();
    }
}
