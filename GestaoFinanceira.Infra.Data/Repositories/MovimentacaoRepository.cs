using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories
{
    public class MovimentacaoRepository : GenericWriteRepository<Movimentacao>, IMovimentacaoRepository
    {
        public MovimentacaoRepository(SqlContext context):base(context)
        {
            this.context = context;

        }

        public override void Delete(int idUsuario)
        {
            dbset.RemoveRange(dbset.Where(m => m.ItemMovimentacao.Categoria.IdUsuario==idUsuario));
        }

        public Movimentacao GetByKey(int idItMov, DateTime dataReferencia)
        {
            return dbset.Where(m => m.IdItemMovimentacao == idItMov && m.DataReferencia == dataReferencia)
                        .Include(m => m.MovimentacaoPrevista)
                        .Include(m => m.MovimentacoesRealizadas)
                        .FirstOrDefault();

        }
    }
}
