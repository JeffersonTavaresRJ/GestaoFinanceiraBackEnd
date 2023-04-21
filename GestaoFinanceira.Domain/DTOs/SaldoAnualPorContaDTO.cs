using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class SaldoAnualPorContaDTO
    {
        public int IdConta { get; set; }
        public string DescricaoConta { get; set; }
        public int Ano { get; set; }
        public float DezembroAnterior { get; set; }
        public float Janeiro { get; set; }
        public float PercJaneiro { get; set; }
        public float Fevereiro { get; set; }
        public float PercFevereiro { get; set; }
        public float Marco { get; set; }
        public float PercMarco { get; set; }
        public float Abril { get; set; }
        public float PercAbril { get; set; }
        public float Maio { get; set; }
        public float PercMaio { get; set; }
        public float Junho { get; set; }
        public float PercJunho { get; set; }
        public float Julho { get; set; }
        public float PercJulho { get; set; }
        public float Agosto { get; set; }
        public float PercAgosto { get; set; }
        public float Setembro { get; set; }
        public float PercSetembro { get; set; }
        public float Outubro { get; set; }
        public float PercOutubro { get; set; }
        public float Novembro { get; set; }
        public float PercNovembro { get; set; }
        public float Dezembro { get; set; }
        public float PercDezembro { get; set; }
    }
}
