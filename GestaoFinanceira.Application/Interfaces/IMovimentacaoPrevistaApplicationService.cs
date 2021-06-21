using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IMovimentacaoPrevistaApplicationService
    {
        Task Add(CreateMovimentacaoPrevistaCommand command);
        Task Update(UpdateMovimentacaoPrevistaCommand command);
        Task Delete(DeleteMovimentacaoPrevistaCommand command);
    }
}
