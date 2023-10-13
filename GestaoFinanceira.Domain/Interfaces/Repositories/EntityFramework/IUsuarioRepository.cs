using System;
using System.Collections.Generic;
using System.Text;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Usuario Get(string email);
        Usuario Get(string email, string senha);
        void TrocaSenha(Usuario usuario);
    }
}
