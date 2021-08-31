using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class MovimentacaoParcelada
    {
        public int IdItemMovimentacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public int ParcelaAtual { get; set; }
        public int ParcelaTotal { get; set; }

        public virtual Movimentacao Movimentacao { get; set; }
    }
}
