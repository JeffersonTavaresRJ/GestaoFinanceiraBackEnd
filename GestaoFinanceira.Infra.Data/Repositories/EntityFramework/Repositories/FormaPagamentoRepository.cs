﻿using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class FormaPagamentoRepository : GenericRepository<FormaPagamento>, IFormaPagamentoRepository
    {
        public FormaPagamentoRepository(SqlContext sqlContext) : base(sqlContext)
        {

        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(c => c.IdUsuario == idUsuario));
        }

        public override IEnumerable<FormaPagamento> GetAll(int idUsuario)
        {
            return dbset.Where(f => f.IdUsuario == idUsuario);
        }
    }
}
