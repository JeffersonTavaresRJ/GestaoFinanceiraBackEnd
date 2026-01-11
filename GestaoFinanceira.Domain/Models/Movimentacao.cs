using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Models
{
    public class Movimentacao
    {
        
        public int IdItemMovimentacao { get; set; }      
        public DateTime DataReferencia { get; set; }
        public TipoPrioridade TipoPrioridade { get; set; }

        public virtual ItemMovimentacao ItemMovimentacao { get; set; }
        public virtual List<MovimentacaoPrevista> MovimentacoesPrevistas { get; set; }
        public virtual List<MovimentacaoRealizada> MovimentacoesRealizadas { get; set; }
    }
}
