using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class MovimentacaoRealizada
    {
        public int Id { get; set; }
        public int IdItemMovimentacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdConta { get; set; }

        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Conta Conta { get; set; }
        public virtual Movimentacao Movimentacao { get; set; }
    }
}
