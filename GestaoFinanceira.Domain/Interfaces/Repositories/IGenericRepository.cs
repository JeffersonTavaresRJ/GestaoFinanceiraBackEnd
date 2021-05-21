using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        void Delete(int idUsuario);
        TEntity GetId(int id);
        abstract IEnumerable<TEntity> GetAll(int idUsuario);

    }
}
