using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Exceptions.Usuario
{
    public class UsuarioInvalidoException : Exception
    {
        public override string Message => "Usuário inválido";
    }
}
