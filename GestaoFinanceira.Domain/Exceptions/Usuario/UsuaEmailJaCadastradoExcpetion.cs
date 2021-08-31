using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.Usuario
{
    public class UsuaEmailJaCadastradoExcpetion : Exception
    {
        private string email;

        public UsuaEmailJaCadastradoExcpetion(string email)
        {
            this.email = email;
        }

        public override string Message => $"O e-mail '{this.email}' encontra-se cadastrado para outro usuário";
    }
}
