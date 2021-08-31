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

    public class MovimentacaoRealizadaCommand
    {
        public int IdItemMovimentacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataMovimentacaoRealizada { get; set; }
        public string TipoPrioridade { get; set; }
        public string Observacao { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdConta { get; set; }
    }
}
