using GestaoFinanceira.Application.Commands.Fechamento;
using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IFechamentoApplicationService
    {
        Task Executar(CreateFechamentoCommand fechamentoCreateCommand);
        List<FechamentoMensalDTO> GetAll();
    }
}
