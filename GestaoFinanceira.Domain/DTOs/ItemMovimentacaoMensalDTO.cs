using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class ItemMovimentacaoMensalDTO
    {
        public DateTime DataReferencia { get; set; }
        public int IdConta { get; set; }
        public string DescricaoConta { get; set; }
        public int IdCategoria { get; set; }
        public string DescricaoCategoria { get; set; }
        public int IdItemMovimentacao { get; set; }
        public string DescricaoItemMovimentacao { get; set; }
        public string TipoItemMovimentacao { get; set; }
        public string DescricaoTipoItemMovimentacao { get; set; }
        public string TipoOperacao { get; set; }
        public float ValorMensal { get; set; }
        public float DiferencaPercentualMensal { get; set; }
    }
}
