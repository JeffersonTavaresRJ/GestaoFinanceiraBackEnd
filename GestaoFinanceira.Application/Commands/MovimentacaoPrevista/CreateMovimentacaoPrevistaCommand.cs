using GestaoFinanceira.Application.Commands.Movimentacao;
using MediatR;
using System;

namespace GestaoFinanceira.Application.Commands.MovimentacaoPrevista
{
    public class CreateMovimentacaoPrevistaCommand :  IRequest
    {
        public int IdItemMovimentacao { get; set; }
        public string TipoPrioridade { get; set; }
        public string Observacao { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public string TipoRecorrencia { get; set; }
        public int QtdeParcelas { get; set; }
    }
    
}
