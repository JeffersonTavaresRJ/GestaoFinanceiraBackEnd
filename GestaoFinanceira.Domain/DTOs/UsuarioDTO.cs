using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string AccessToken { get; set; }
    }
}
