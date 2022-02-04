using GestaoFinanceira.Application.Commands.Movimentacao;
using MediatR;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Application.Commands.MovimentacaoPrevista
{
    public class CreateMovimentacaoPrevistaCommand: IRequest
    {
        public List<MovimentacaoPrevistaCommand> MovimentacaoPrevistaCommand { get; set; }
    }


    public class MovimentacaoPrevistaCommand :  IRequest
    {
        public int IdItemMovimentacao { get; set; }
        public string TipoPrioridade { get; set; }
        public string Observacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int NrParcela { get; set; }
        public int NrParcelaTotal { get; set; }
    }
    
}
