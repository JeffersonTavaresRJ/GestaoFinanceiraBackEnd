using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public Movimentacao GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            return dbset.Where(m => m.IdItemMovimentacao == idItemMovimentacao && m.DataReferencia == dataReferencia)
                        .Include(m => m.MovimentacoesPrevistas)
                        .Include(m => m.MovimentacoesRealizadas)
                        .Include(m=>  m.ItemMovimentacao)
                        .Include(m => m.ItemMovimentacao.Categoria)
                        .FirstOrDefault();

        }
    }
}
