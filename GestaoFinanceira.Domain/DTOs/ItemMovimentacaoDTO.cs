using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class ItemMovimentacaoDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string TipoDescricao { get; set; }
        public bool Status { get; set; }
        public virtual CategoriaDTO Categoria { get; set; }
        public string TipoOperacao { get; set; }
    }
}
