using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class MovimentacaoDTO
    {
        public ItemMovimentacaoDTO ItemMovimentacao { get; set; }
        public DateTime DataReferencia { get; set; }
        public string TipoPrioridade { get; set; }
        public string TipoPrioridadeDescricao { get; set; }
        public string Observacao { get; set; }
    }
}
