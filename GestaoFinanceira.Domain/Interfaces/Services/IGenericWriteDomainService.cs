using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IGenericWriteDomainService<TEntity>
        where TEntity : class
    {
        int Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
    }
}
