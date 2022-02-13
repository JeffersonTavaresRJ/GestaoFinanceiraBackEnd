using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IMovimentacaoRealizadaApplicationService
    {
        Task Add(CreateMovimentacaoRealizadaCommand command);
        Task Update(UpdateMovimentacaoRealizadaCommand command);
        Task Delete(DeleteMovimentacaoRealizadaCommand command);   
        MovimentacaoRealizadaDTO GetId(int id);
        List<MovimentacaoRealizadaDTO> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, int idUsuario, DateTime dataMovRealIni, DateTime dataMovRealFim);
        List<SaldoDiarioDTO> GetGroupBySaldoDiario(int idUsuario, DateTime dataMovRealIni, DateTime dataMovRealFim);
    }
}
