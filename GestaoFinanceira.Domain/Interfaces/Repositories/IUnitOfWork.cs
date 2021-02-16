using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork :IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();

        IUsuarioRepository IUsuarioRepository { get; }
        ICategoriaRepository ICategoriaRepository { get; }

    }
}
