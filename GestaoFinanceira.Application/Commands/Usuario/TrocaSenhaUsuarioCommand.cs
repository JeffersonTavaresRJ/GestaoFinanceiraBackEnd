using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Usuario
{
    public class TrocaSenhaUsuarioCommand
    {
        public int Id { get; set; }
        public string SenhaAtual { get; set; }
        public string Senha { get; set; }
    }
}
