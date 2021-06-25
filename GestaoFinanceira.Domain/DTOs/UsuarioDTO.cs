using MongoDB.Bson.Serialization.Attributes;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string AccessToken { get; set; }
    }
}
