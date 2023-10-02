using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Commands.SaldoMensalConta;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Reports.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class MovimentacaoRealizadaApplicationService : IMovimentacaoRealizadaApplicationService
    {
        private readonly IMediator mediator;
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly IMovimentacaoRealizadaMensalCaching movimentacaoRealizadaMensalCaching;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;

        public MovimentacaoRealizadaApplicationService(IMediator mediator, 
                                                       IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching,
                                                       IMovimentacaoRealizadaMensalCaching movimentacaoRealizadaMensalCaching,
                                                       ISaldoDiarioCaching saldoDiarioCaching)
        {
            this.mediator = mediator;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
            this.movimentacaoRealizadaMensalCaching = movimentacaoRealizadaMensalCaching;
            this.saldoDiarioCaching = saldoDiarioCaching;
        }

        public Task Add(CreateMovimentacaoRealizadaCommand command)
        {
            return mediator.Send(command);
        }

        public Task Update(UpdateMovimentacaoRealizadaCommand command)
        {
            return mediator.Send(command);
        }

        public Task Delete(DeleteMovimentacaoRealizadaCommand command)
        {
            return mediator.Send(command);
        }

        public async Task<List<SaldoContaMensalDTO>> GetSaldoMensalPorConta(ReaderSaldoMensalPorContaCommand command)
        {
            return await mediator.Send(command); 
        }

        public async Task<List<SaldoContaAnualDTO>> GetSaldoAnualPorConta(ReaderSaldoAnualPorContaCommand command)
        {
            return await mediator.Send(command);
        }

        public async Task<List<ItemMovimentacaoMensalDTO>> GetItemMovimentacaoMensal(ReaderItemMovimentacaoMensalCommand command)
        {
            return await mediator.Send(command);
        }

        public MovimentacaoRealizadaDTO GetId(int id)
        {
            return movimentacaoRealizadaCaching.GetId(id);
        }

        public List<MovimentacaoRealizadaDTO> GetByDataReferencia(int? idItemMovimentacao, DateTime dataReferencia)
        {
            return movimentacaoRealizadaCaching.GetByDataReferencia(idItemMovimentacao, dataReferencia);
        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            return movimentacaoRealizadaCaching.GetByDataMovimentacaoRealizada(idItemMovimentacao, dataMovRealIni, dataMovRealFim);
        }

        public List<SaldoDiarioDTO> GetGroupBySaldoDiario(DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            return saldoDiarioCaching.GetGroupBySaldoDiario(dataMovRealIni, dataMovRealFim);
        }

        public List<SaldoDiarioDTO> GetMaxGroupBySaldoConta(DateTime dataReferencia)
        {
            return saldoDiarioCaching.GetMaxGroupBySaldoConta(dataReferencia); 
        }

        public byte[] GetByMovimentacaoRealizadaMensal(List<int> idsConta, DateTime dataReferencia)
        {
            List<MovimentacaoRealizadaMensalDTO> movimentacaoRealizadaMensalDTOs 
                = movimentacaoRealizadaMensalCaching.GetByMovimentacaoRealizadaMensal(idsConta, dataReferencia);
            return ReportMovimentacaoRealizadaMensal.GetAll(movimentacaoRealizadaMensalDTOs);
        }
    }
}