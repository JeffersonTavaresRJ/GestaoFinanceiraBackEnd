using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IGenericDomainService<TEntity> where TEntity: class
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        TEntity GetId(int id);
        abstract List<TEntity> GetAll(int idUsuario);
    }
}
