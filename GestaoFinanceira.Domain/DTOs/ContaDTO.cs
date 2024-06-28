using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class ContaDTO : GenericDTO
    {
        public string DefaultConta { get; set; }
        public string Tipo { get; set; }
        public int IdUsuario { get; set; }
    }
}
