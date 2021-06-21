using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IGenericDomainService<TEntity>: IGenericWriteDomainService<TEntity>
        where TEntity: class
    {
        
        TEntity GetId(int id);
        abstract List<TEntity> GetAll(int idUsuario);
    }
}
