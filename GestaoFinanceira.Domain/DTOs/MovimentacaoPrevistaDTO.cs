using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class MovimentacaoPrevistaDTO : MovimentacaoDTO
    {
        public FormaPagamentoDTO FormaPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public string StatusDescricao { get; set; }

    }
}
