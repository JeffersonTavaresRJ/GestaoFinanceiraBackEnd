using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IGenericWriteRepository<TEntity> : IDisposable
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        void Delete(int idUsuario);
    }
}
