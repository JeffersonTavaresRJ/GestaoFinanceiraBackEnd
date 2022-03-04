using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IGenericCaching<TEntity> : IGenericWriteCaching<TEntity>
        where TEntity: class
    {        
        TEntity GetId(int id);
        List<TEntity> GetAll();
    }
}
