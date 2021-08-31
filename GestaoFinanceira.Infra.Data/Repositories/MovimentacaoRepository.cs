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
                        .Include(m => m.MovimentacaoPrevista).ThenInclude(x => x.FormaPagamento)
                        .Include(m => m.MovimentacoesRealizadas).ThenInclude(x=>x.FormaPagamento)
                        .Include(m => m.MovimentacoesRealizadas).ThenInclude(x => x.Conta)
                        .Include(m=>  m.ItemMovimentacao)
                        .Include(m => m.ItemMovimentacao.Categoria)
                        .FirstOrDefault();

        }
    }
}
