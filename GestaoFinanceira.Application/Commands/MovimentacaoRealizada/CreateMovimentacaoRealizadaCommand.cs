using GestaoFinanceira.Application.Commands.Movimentacao;
using MediatR;
using System;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class CreateMovimentacaoRealizadaCommand : MovimentacaoCommand, IRequest
    {
        public DateTime DataMovimentacaoRealizada { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdConta { get; set; }
        public int? IdMovimentacaoPrevista { get; set; }
        public string StatusMovimentacaoPrevista { get; set; }
    }
}
