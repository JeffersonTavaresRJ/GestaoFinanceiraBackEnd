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
            try
            {
                repository.Add(obj);
            }
            catch (Exception e)
            {

                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
            
        }

        public virtual void Update(TEntity obj)
        {
            try
            {
                repository.Update(obj);
            }            
            catch (Exception e)
            {
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public virtual void Delete(TEntity obj)
        {
            try
            {
                repository.Delete(obj);
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException != null ? e.InnerException.Message : e.Message);
            }

        }
        public virtual void Dispose()
        {
            repository.Dispose();
        }
    }
}
