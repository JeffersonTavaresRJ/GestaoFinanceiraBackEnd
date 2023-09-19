using MongoDB.Bson.Serialization.Attributes;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class CategoriaDTO : GenericDTO
    {
        public int IdUsuario { get; set; }
    }
}