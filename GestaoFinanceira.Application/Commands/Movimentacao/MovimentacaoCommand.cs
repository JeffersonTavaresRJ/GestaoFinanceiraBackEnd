using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Movimentacao
{
    public class MovimentacaoCommand
    {
        public int IdItemMovimentacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public string TipoPrioridade { get; set; }
        public string Observacao { get; set; }
    }
}
