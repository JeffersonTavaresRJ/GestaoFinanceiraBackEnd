using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class GenericDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
    }
}
