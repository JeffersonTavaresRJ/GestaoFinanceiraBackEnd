using GestaoFinanceira.Application.Commands.Movimentacao;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.MovimentacaoPrevista
{
    public class UpdateMovimentacaoPrevistaCommand : MovimentacaoCommand, IRequest
    {
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public int IdFormaPagamento { get; set; }
    }
}
