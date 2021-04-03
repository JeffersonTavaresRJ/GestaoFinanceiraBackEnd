using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Exceptions.Usuario
{
    public class SenhaInvalidaException :Exception
    {
       public override string Message => "Senha Inválida";
    }
}
