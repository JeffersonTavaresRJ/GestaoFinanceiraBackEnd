using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Commands.SaldoAnual;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IMovimentacaoRealizadaApplicationService
    {
        Task Add(CreateMovimentacaoRealizadaCommand command);
        Task Update(UpdateMovimentacaoRealizadaCommand command);
        Task Delete(DeleteMovimentacaoRealizadaCommand command);   
        MovimentacaoRealizadaDTO GetId(int id);
        List<MovimentacaoRealizadaDTO> GetByDataReferencia(int? idItemMovimentacao, DateTime? dataReferencia);
        List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, DateTime dataMovRealIni, DateTime dataMovRealFim);
        List<SaldoDiarioDTO> GetGroupBySaldoDiario(DateTime dataMovRealIni, DateTime dataMovRealFim);
        List<SaldoDiarioDTO> GetMaxGroupBySaldoConta(DateTime? dataReferencia);
        Task<List<SaldoAnualPorContaDTO>> GetSaldoAnualPorConta(ReaderSaldoAnualPorContaCommand command);
        Task<List<SaldoAnualPorPeriodoDTO>> GetSaldoAnualPorPeriodo(ReaderSaldoAnualPorPeriodoCommand command);
    }
}
