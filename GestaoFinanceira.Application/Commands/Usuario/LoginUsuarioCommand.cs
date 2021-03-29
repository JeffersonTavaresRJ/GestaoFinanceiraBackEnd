using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Usuario
{
    public class LoginUsuarioCommand
    {
        public string EMail { get; set; }
        public string Senha { get; set; }
    }
}
