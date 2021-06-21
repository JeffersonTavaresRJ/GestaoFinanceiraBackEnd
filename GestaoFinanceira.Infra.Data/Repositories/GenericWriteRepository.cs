using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public abstract class GenericWriteRepository<TEntity>: IGenericWriteRepository<TEntity>
        where TEntity:class
    {
        protected SqlContext context;
        protected DbSet<TEntity> dbset;

        protected GenericWriteRepository(SqlContext context)
        {
            this.context = context;
            dbset = context.Set<TEntity>();
        }

        public virtual void Add(TEntity obj)
        {
            
            dbset.Add(obj);
            context.SaveChanges();
        }

        public virtual void Update(TEntity obj)
        {
            dbset.Update(obj);
            context.SaveChanges();
        }

        public virtual void Delete(TEntity obj)
        {
            dbset.Remove(obj);
            context.SaveChanges();
        }        

        public void Dispose()
        {
            context.Dispose();
        }
        public abstract void Delete(int idUsuario);
    }
}
