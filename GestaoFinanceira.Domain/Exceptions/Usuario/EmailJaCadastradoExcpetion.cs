using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.Usuario
{
    public class EmailJaCadastradoExcpetion : Exception
    {
        private string email;

        public EmailJaCadastradoExcpetion(string email)
        {
            this.email = email;
        }

        public override string Message => $"O e-mail '{this.email}' encontra-se cadastrado para outro usuário";
    }
}
