using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class SaldoContaAnual
    {
        public int IdUsuario { get; set; }
        public int IdConta { get; set; }
        public string DescricaoConta { get; set; }
        public int Ano { get; set; }
        public int TotalMeses { get; set; }
        public float Saldo { get; set; }
        public float ReceitaAnual { get; set; }
    }
}
