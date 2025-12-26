using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Models
{
    public class MovimentacaoPrevista
    {
        public int Id { get; set; }
        public int IdItemMovimentacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public StatusMovimentacaoPrevista Status { get; set; }
        public int IdFormaPagamento { get; set; }
        public int? IdMovPrevParcelada { get; set; }
        public int NrParcela { get; set; }
        public int NrParcelaTotal { get; set; }
        public string Observacao { get; set; }

        public virtual Movimentacao Movimentacao { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual List<MovimentacaoRealizada> MovimentacoesRealizadas { get; set; }
    }
}
