using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public class FormaPagamentoDTO : GenericDTO
    {
        public int IdUsuario { get; set; }
    }
}
