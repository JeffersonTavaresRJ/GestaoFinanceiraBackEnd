using MongoDB.Bson.Serialization.Attributes;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public int IdUsuario { get; set; }
    }
}