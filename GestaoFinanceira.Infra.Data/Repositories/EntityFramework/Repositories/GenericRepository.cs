using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public abstract class GenericRepository<TEntity> : GenericWriteRepository<TEntity>, IGenericRepository<TEntity>
        where TEntity : class
    {

        protected GenericRepository(SqlContext context) : base(context)
        {
            /*consulta sem acompanhamento de leitura..*/
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public virtual TEntity GetId(int id)
        {
            return dbset.Find(id);
        }

        public abstract IEnumerable<TEntity> GetAll(int idUsuario);

    }
}
