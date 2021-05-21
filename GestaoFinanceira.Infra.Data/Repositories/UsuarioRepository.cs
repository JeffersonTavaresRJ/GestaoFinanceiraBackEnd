using GestaoFinanceira.Infra.Data.Context;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SqlContext context) : base(context)
        {

        }


        public Usuario Get(string email)
        {
             return dbset.FirstOrDefault(u => u.EMail.Equals(email));
        }

        public Usuario Get(string email, string senha)
        {
            return dbset.FirstOrDefault(u => u.EMail.Equals(email) && u.Senha.Equals(senha));
        }

        public override IEnumerable<Usuario> GetAll(int idUsuario)
        {
            return dbset.Where(u => u.Id == idUsuario || idUsuario == 0);
        }
        
        public void TrocaSenha(Usuario usuario)
        {
            context.Usuarios.Attach(usuario);
            context.Entry(usuario).Property(u => u.Senha).IsModified = true;
            context.SaveChanges();
        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(u => u.Id == idUsuario));
        }
    }
}
