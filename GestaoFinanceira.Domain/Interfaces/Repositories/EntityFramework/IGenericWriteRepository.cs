using System;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IGenericWriteRepository<TEntity> : IDisposable
    {
        int Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        void Delete(int idUsuario);
    }
}
