using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections;
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
        MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoPrevistaDTO> GetByDataReferencia(int? idItemMovimentacao, int idUsuario, DateTime dataRefIni, DateTime dataRefFim);
        IList GetAllStatus();

    }
}
