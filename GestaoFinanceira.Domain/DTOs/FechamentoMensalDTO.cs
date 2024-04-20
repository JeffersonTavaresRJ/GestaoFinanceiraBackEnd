using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class FechamentoMensalDTO
    {
        public string MesAno { get; set; }
        public DateTime DataReferencia { get; set; }
        public string Status { get; set; }
        public string DescricaoStatus { get; set; }
    }
}
