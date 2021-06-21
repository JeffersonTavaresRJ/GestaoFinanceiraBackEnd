using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class GenericWriteDomainService<TEntity> : IGenericWriteDomainService<TEntity>
        where TEntity:class
    {
        protected readonly IGenericWriteRepository<TEntity> repository;

        public GenericWriteDomainService(IGenericWriteRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual void Add(TEntity obj)
        {
            repository.Add(obj);
        }

        public virtual void Update(TEntity obj)
        {
            repository.Update(obj);
        }

        public virtual void Delete(TEntity obj)
        {
            repository.Delete(obj);
        }
        public virtual void Dispose()
        {
            repository.Dispose();
        }
    }
}
