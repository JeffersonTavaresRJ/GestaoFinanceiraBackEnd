using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IMovimentacaoPrevistaApplicationService
    {
        Task Add(CreateMovimentacaoPrevistaCommand command);
        Task Update(UpdateMovimentacaoPrevistaCommand command);
        Task Delete(DeleteMovimentacaoPrevistaCommand command);
        MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime? dataVencIni, DateTime? dataVencFim, int? idItemMovimentacao);
        List<GenericEnum> GetAllStatus();
        List<GenericEnum> GetAllPrioridades();
        List<GenericEnum> GetAllTipoRecorrencias();

    }
}
