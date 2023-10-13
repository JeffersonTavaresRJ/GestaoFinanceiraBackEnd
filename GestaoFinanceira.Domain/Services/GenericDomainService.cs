using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public abstract class GenericDomainService<TEntity> : GenericWriteDomainService<TEntity>
        where TEntity :class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected GenericDomainService(IGenericRepository<TEntity> repository):base(repository)
        {
            this._repository = repository;
        }

       
        public virtual TEntity GetId(int id)
        {
            return _repository.GetId(id);
        }

        public abstract List<TEntity> GetAll(int idUsuario);

        
    }
}
