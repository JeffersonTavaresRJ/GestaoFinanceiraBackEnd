using AutoMapper;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Handlers
{
    public class MovimentacaoRealizadaHandler : INotificationHandler<MovimentacaoRealizadaNotification>
    {
        private readonly IMapper mapper;
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly IFormaPagamentoCaching formaPagamentoCaching;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;
        private readonly IContaCaching contaCaching;
        private MovimentacaoRealizadaDTO movimentacaoRealizadaDTO;

        public MovimentacaoRealizadaHandler(IMapper mapper, IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching,
            IFormaPagamentoCaching formaPagamentoCaching, IItemMovimentacaoCaching itemMovimentacaoCaching, IContaCaching contaCaching)
        {
            this.mapper = mapper;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
            this.formaPagamentoCaching = formaPagamentoCaching;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
            this.contaCaching = contaCaching;
        }

        public Task Handle(MovimentacaoRealizadaNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {

                switch (notification.Action)
                {
                    case ActionNotification.Criar:
                        foreach (MovimentacaoRealizada MovimentacaoRealizada in notification.MovimentacoesRealizadas)
                        {
                            movimentacaoRealizadaDTO = Convert(MovimentacaoRealizada);
                            movimentacaoRealizadaCaching.Add(movimentacaoRealizadaDTO);
                        }
                        break;
                    case ActionNotification.Atualizar:
                        movimentacaoRealizadaDTO = Convert(notification.MovimentacaoRealizada);
                        movimentacaoRealizadaCaching.Update(movimentacaoRealizadaDTO);
                        break;
                    case ActionNotification.Excluir:
                        movimentacaoRealizadaDTO = Convert(notification.MovimentacaoRealizada);
                        movimentacaoRealizadaCaching.Delete(movimentacaoRealizadaDTO);
                        break;
                }
            });
        }

        private MovimentacaoRealizadaDTO Convert(MovimentacaoRealizada MovimentacaoRealizada)
        {
            MovimentacaoRealizadaDTO MovimentacaoRealizadaDTO = mapper.Map<MovimentacaoRealizadaDTO>(MovimentacaoRealizada);
            MovimentacaoRealizadaDTO.FormaPagamento = formaPagamentoCaching.GetId(MovimentacaoRealizada.IdFormaPagamento);
            MovimentacaoRealizadaDTO.ItemMovimentacao = itemMovimentacaoCaching.GetId(MovimentacaoRealizada.IdItemMovimentacao);
            MovimentacaoRealizadaDTO.Conta = contaCaching.GetId(MovimentacaoRealizada.IdConta);
            return MovimentacaoRealizadaDTO;
        }
    }
}