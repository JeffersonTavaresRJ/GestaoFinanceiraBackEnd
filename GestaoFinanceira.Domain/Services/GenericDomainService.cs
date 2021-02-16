using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public abstract class GenericDomainService<TEntity> : IGenericDomainService<TEntity>
        where TEntity :class
    {
        protected readonly IGenericRepository<TEntity> repository;

        protected GenericDomainService(IGenericRepository<TEntity> repository)
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

        public virtual TEntity GetId(int id)
        {
            return repository.GetId(id);
        }

        public abstract List<TEntity> GetAll(int idUsuario);

        public void Dispose()
        {
            repository.Dispose();
        }
    }
}
