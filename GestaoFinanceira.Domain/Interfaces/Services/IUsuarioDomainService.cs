using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IUsuarioDomainService :IGenericDomainService<Usuario>
    {
        Usuario Get(string email);
        Usuario Get(string email, string senha);
        void TrocaSenha(Usuario usuario);
    }
}
