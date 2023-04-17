using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class SaldoAnual
    {
        public int IdUsuario { get; set; }
        public int IdConta { get; set; }
        public string DescricaoConta { get; set; }
        public int Ano { get; set; }
        public float Janeiro { get; set; }
        public float Fevereiro { get; set; }
        public float Marco { get; set; }
        public float Abril { get; set; }
        public float Maio { get; set; }
        public float Junho { get; set; }
        public float Julho { get; set; }
        public float Agosto { get; set; }
        public float Setembro { get; set; }
        public float Outubro { get; set; }
        public float Novembro { get; set; }
        public float Dezembro { get; set; }
    }
}
