using System;

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
