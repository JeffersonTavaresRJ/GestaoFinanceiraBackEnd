using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Usuario
{
    public class UpdateUsuarioCommand
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string Senha { get; set; }
    }
}
