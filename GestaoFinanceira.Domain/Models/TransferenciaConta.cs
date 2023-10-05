using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class TransferenciaContas
    {
        public int IdConta { get; set; }
        public int IdContaDestino { get; set; }
        public DateTime DataMovimentacaoRealizada { get; set; }
        public double Valor { get; set; }
        public string Observacao { get; set; }
    }
}
