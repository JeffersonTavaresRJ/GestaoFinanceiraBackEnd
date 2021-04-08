using System;
using System.Collections.Generic;
using System.Text;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {          
        Usuario Get(string email);
        Usuario Get(string email, string senha);
        void UpdateByCadastro(Usuario usuario);
        void UpdateBySenha(Usuario usuario);
    }
}
