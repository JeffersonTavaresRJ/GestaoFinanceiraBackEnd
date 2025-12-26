using System;

namespace GestaoFinanceira.Domain.DTOs
{
    public class MovimentacaoRealizadaDTO : MovimentacaoDTO
    {
        public ContaDTO Conta { get; set; }
        public FormaPagamentoDTO FormaPagamento { get; set; }
        public int Id { get; set; }
        public DateTime DataMovimentacaoRealizada { get; set; }
        public double Valor { get; set; }
        public int IdMovimentacaoPrevista { get; set; }


    }
}
