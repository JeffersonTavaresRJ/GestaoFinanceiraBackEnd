using GestaoFinanceira.Application.Commands.Movimentacao;
using MediatR;
using System;

namespace GestaoFinanceira.Application.Commands.MovimentacaoPrevista
{
    public class CreateMovimentacaoPrevistaCommand : MovimentacaoCommand, IRequest
    {
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public int IdFormaPagamento { get; set; }
        public int QtdeParcelas { get; set; }
    }
}
