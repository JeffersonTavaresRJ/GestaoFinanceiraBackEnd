using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public virtual int Add(TEntity obj)
        {
            
            dbset.Add(obj);
            context.SaveChanges();
            return obj.GetType().GetProperty("Id") != null && obj.GetType().GetProperty("Id").GetValue(obj, null) != null ? (int)obj.GetType().GetProperty("Id").GetValue(obj, null) : 0;            
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
