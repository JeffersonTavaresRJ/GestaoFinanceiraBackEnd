using GestaoFinanceira.Application.Commands.Movimentacao;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class CreateMovimentacaoRealizadaCommand : IRequest
    {
        public List<MovimentacaoRealizadaCommand> MovimentacaoRealizadaCommand { get; set; }
    }

    public class MovimentacaoRealizadaCommand : MovimentacaoCommand
    {
        public DateTime DataMovimentacaoRealizada { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdConta { get; set; }
    }
}
