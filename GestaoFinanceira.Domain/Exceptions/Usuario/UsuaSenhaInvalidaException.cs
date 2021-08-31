using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.Usuario
{
    public class UsuaSenhaInvalidaException : Exception
    {
        public override string Message => "Senha inválida";
    }
}
