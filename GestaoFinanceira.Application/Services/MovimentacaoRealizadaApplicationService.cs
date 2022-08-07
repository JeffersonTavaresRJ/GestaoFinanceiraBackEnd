using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GestaoFinanceira.Application.Services
{
    public class MovimentacaoRealizadaApplicationService : IMovimentacaoRealizadaApplicationService
    {
        private readonly IMediator mediator;
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;

        public MovimentacaoRealizadaApplicationService(IMediator mediator, IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching, ISaldoDiarioCaching saldoDiarioCaching)
        {
            this.mediator = mediator;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
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
        public MovimentacaoRealizadaDTO GetId(int id)
        {
            return movimentacaoRealizadaCaching.GetId(id);
        }

        public List<MovimentacaoRealizadaDTO> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia)
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
            var dataIni = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
            var dataFim = new DateTime(dataReferencia.Year, dataReferencia.Month, DateTime.DaysInMonth(dataReferencia.Year, dataReferencia.Month));

            List<SaldoDiarioDTO> saldosDiario = saldoDiarioCaching.GetGroupBySaldoDiario(dataIni, dataFim);
            //List<SaldoDiarioFechamentoDTO> result = new List<SaldoDiarioFechamentoDTO>();

            //foreach (var item in saldosDiario)
            //{
            //    var model = new SaldoDiarioFechamentoDTO();
            //    model.Conta = item.Conta.Id;
            //    model.Valor = item.Valor;
            //    model.DataSaldo = item.DataSaldo;
            //    result.Add(model);
            //}

            //var xpto =  from sd in saldosDiario
            //           group sd by sd.Conta into groupConta
            //           orderby groupConta.Key ascending
            //           select groupConta;



            saldosDiario = saldosDiario
                         .Select(s => new SaldoDiarioDTO
                         {
                             Conta = s.Conta,
                             Valor = s.Valor,
                             DataSaldo = (from x in saldosDiario select x.DataSaldo).Max()
                         }).ToList();

            return saldosDiario;

        }
    }
}
