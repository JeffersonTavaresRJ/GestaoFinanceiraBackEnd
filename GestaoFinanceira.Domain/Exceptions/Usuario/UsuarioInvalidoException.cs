using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.Usuario
{
    public class UsuarioInvalidoException : Exception
    {
        public override string Message => "Usuário inválido";
    }
}
