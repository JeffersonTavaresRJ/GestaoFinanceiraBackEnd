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
        public float Saldo { get; set; }
        public float ValorInicial { get; set; }
        public float ReceitaTotalAnual { get; set; }
        public float ReceitaMediaMensal { get; set; }
        public float SaldoEsperado { get; set; }
    }
}
