using System;

namespace GestaoFinanceira.Domain.Models
{
    public class SaldoDiario
    {
        public int IdConta { get; set; }
        public DateTime DataSaldo { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public virtual Conta Conta { get; set; }
    }
}