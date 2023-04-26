using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class ItemMovimentacaoMensal
    {
        public int IdUsuario { get; set; }
        public DateTime DataReferencia { get; set; }
        public int IdCategoria { get; set; }
        public string DescricaoCategoria { get; set; }
        public int IdItemMovimentacao { get; set; }
        public string DescricaoItemMovimentacao { get; set; }
        public string TipoItemMovimentcao { get; set; }
        public string DescricaoTipoItemMovimentcao { get; set; } 
        public float ValorMensal { get; set; }
        public float DiferencaPercentualMensal { get; set; }
    }
}
