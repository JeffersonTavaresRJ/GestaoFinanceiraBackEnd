using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IGenericRepository<TEntity> : IGenericWriteRepository<TEntity>
    {
        TEntity GetId(int id);
        abstract IEnumerable<TEntity> GetAll(int idUsuario);

    }
}
