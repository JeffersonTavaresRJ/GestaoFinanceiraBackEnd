using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.DTOs
{
    [BsonIgnoreExtraElements]
    public  class SaldoDiarioDTO
    {
        public ContaDTO Conta { get; set; }
        public DateTime DataSaldo { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public List<MovimentacaoRealizadaDTO> MovimentacoesRealizadas { get; set; }
    }
}
