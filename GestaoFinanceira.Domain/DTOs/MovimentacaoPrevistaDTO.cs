using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class MovimentacaoPrevistaDTO : MovimentacaoDTO
    {
        public int Id { get; set; }
        public FormaPagamentoDTO FormaPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public string StatusDescricao { get; set; }
        public string Parcela { get; set; }

    }
}
