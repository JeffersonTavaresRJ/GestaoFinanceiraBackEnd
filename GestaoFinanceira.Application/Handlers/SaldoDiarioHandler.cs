using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class SaldoDiarioHandler : INotificationHandler<SaldoDiarioNotification>
    {
        private readonly IMapper mapper;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly IContaCaching contaCaching;
        public SaldoDiarioHandler(IMapper mapper, ISaldoDiarioCaching saldoDiarioCaching, 
                                                  IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching,
                                                  IContaCaching contaCaching)
        {
            this.mapper = mapper;
            this.saldoDiarioCaching = saldoDiarioCaching;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
            this.contaCaching = contaCaching;           
        }

        public Task Handle(SaldoDiarioNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        foreach (SaldoDiario saldoDiario in notification.SaldosDiario)
                        {
                            SaldoDiarioDTO saldoDiarioDTO = Convert(saldoDiario);

                            var saldo = saldoDiarioCaching.GetByKey(saldoDiario.IdConta, saldoDiario.DataSaldo);
                            if (saldo == null)
                            {
                                /*se não existe, cria..*/
                                saldoDiarioCaching.Add(saldoDiarioDTO);
                            }
                            else
                            {
                                /*se já existe, atualiza..*/
                                saldoDiarioCaching.Update(saldoDiarioDTO);
                            }
                        }
                        break;
                    case ActionNotification.Atualizar:
                        foreach (SaldoDiario saldoDiario in notification.SaldosDiario)
                        {
                            if (saldoDiario.IdConta> 0 && saldoDiario.Status == null)
                            {
                                /*O saldo atual foi excluído do banco de dados: lê do mongoDB e exclui..*/
                                SaldoDiarioDTO saldo = saldoDiarioCaching.GetByKey(saldoDiario.IdConta, saldoDiario.DataSaldo);
                                saldoDiarioCaching.Delete(saldo);
                            }
                            else
                            {
                                SaldoDiarioDTO saldoDiarioDTO = Convert(saldoDiario);

                                var saldo = saldoDiarioCaching.GetByKey(saldoDiario.IdConta, saldoDiario.DataSaldo);
                                if (saldo == null)
                                {
                                    /*se não existe, cria..*/
                                    saldoDiarioCaching.Add(saldoDiarioDTO);
                                }
                                else
                                {
                                    /*se já existe, atualiza..*/
                                    saldoDiarioCaching.Update(saldoDiarioDTO);
                                }
                            }                            
                        }
                        break;
                    case ActionNotification.Excluir:
                        foreach (SaldoDiario saldoDiario in notification.SaldosDiario)
                        {
                            if (saldoDiario.IdConta > 0 && saldoDiario.Status == null)
                            {
                                /*O saldo atual foi excluído do banco de dados: lê do mongoDB e exclui..*/
                                SaldoDiarioDTO saldo = saldoDiarioCaching.GetByKey(saldoDiario.IdConta, saldoDiario.DataSaldo);
                                saldoDiarioCaching.Delete(saldo);
                            }
                            else
                            {
                                /*se já existe, atualiza..*/
                                SaldoDiarioDTO saldoDiarioDTO = Convert(saldoDiario);
                                saldoDiarioCaching.Update(saldoDiarioDTO);

                            }
                        }
                        break;
                }
            });
        }

        private SaldoDiarioDTO Convert(SaldoDiario saldoDiario)
        {
            SaldoDiarioDTO saldoDiarioDTO = mapper.Map<SaldoDiarioDTO>(saldoDiario);
            saldoDiarioDTO.Conta = contaCaching.GetId(saldoDiario.IdConta);
            saldoDiarioDTO.MovimentacoesRealizadas = movimentacaoRealizadaCaching.GetByDataMovimentacaoRealizada(saldoDiario.IdConta, saldoDiario.DataSaldo);
            return saldoDiarioDTO;
        }
    }
}
