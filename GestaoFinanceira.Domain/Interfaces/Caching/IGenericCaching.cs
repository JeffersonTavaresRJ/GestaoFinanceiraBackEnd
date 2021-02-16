using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IGenericCaching<TEntity>
        where TEntity: class
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        TEntity GetId(int id);
        List<TEntity> GetAll(int idUsuario);
    }
}
