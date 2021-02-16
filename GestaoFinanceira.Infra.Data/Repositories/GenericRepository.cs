using GestaoFinanceira.Infra.Data.Context;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly SqlContext context;
        protected readonly DbSet<TEntity> dbset;

        protected GenericRepository(SqlContext context)
        {
            this.context = context;
            this.dbset = context.Set<TEntity>();
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

        public virtual TEntity GetId(int id)
        {
            return dbset.Find(id);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public abstract IEnumerable<TEntity> GetAll(int idUsuario);

        
    }
}
