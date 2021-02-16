using System;
using System.Collections.Generic;
using System.Text;
using GestaoFinanceira.Domain.Models.Enuns;

namespace GestaoFinanceira.Domain.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public bool Status { get; set; }
        public int IdUsuario { get; set; }
    }
}