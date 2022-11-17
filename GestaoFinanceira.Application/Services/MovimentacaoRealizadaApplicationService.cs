using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public List<SaldoDiarioDTO> GetMaxGroupBySaldoConta(DateTime? dataReferencia)
        {

            var date = dataReferencia.HasValue ? dataReferencia.Value : saldoDiarioCaching.GetAll().Max(x => x.DataSaldo);
            var dataIni = new DateTime(date.Year, date.Month, 1);
            var dataFim = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            List<SaldoDiarioDTO> saldosDiario = saldoDiarioCaching.GetBySaldosDiario(dataIni, dataFim);

            var listGroupConta = saldosDiario.GroupBy(x => new { x.Conta.Id })
                                    .Select(grp => new
                                    {
                                        grp.Key,
                                        ultimoLancamento = grp.OrderByDescending(x => x.DataSaldo)
                                                              .Select(x => x.DataSaldo)
                                                              .FirstOrDefault()
                                    }).ToList();
            saldosDiario.Clear();

            foreach (var item in listGroupConta)
            {
                SaldoDiarioDTO saldoDiarioDTO = saldoDiarioCaching.GetByKey(item.Key.Id, item.ultimoLancamento);
                saldoDiarioDTO.MovimentacoesRealizadas = null;
                saldosDiario.Add(saldoDiarioDTO);
            }           

            return saldosDiario; 

        }        
    }
}
