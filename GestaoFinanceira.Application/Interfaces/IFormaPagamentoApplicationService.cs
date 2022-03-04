using GestaoFinanceira.Application.Commands.FormaPagamento;
using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IFormaPagamentoApplicationService
    {
        Task Add(CreateFormaPagamentoCommand command);

        Task Update(UpdateFormaPagamentoCommand command);

        Task Delete(DeleteFormaPagamentoCommand command);

        FormaPagamentoDTO GetById(int id);

        List<FormaPagamentoDTO> GetAll();
    }
}
