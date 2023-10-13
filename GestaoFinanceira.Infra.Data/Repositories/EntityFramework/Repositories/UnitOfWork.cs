using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlContext context;
        private IDbContextTransaction transaction;

        public UnitOfWork(SqlContext context)
        {
            this.context = context;
        }

        public void BeginTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public IUsuarioRepository IUsuarioRepository => new UsuarioRepository(context);

        public ICategoriaRepository ICategoriaRepository => new CategoriaRepository(context);

        public IContaRepository IContaRepository => new ContaRepository(context);

        public IFormaPagamentoRepository IFormaPagamentoRepository => new FormaPagamentoRepository(context);

        public IItemMovimentacaoRepository IItemMovimentacaoRepository => new ItemMovimentacaoRepository(context);

        public IMovimentacaoRepository IMovimentacaoRepository => new MovimentacaoRepository(context);

        public IMovimentacaoPrevistaRepository IMovimentacaoPrevistaRepository => new MovimentacaoPrevistaRepository(context);

        public IMovimentacaoRealizadaRepository IMovimentacaoRealizadaRepository => new MovimentacaoRealizadaRepository(context);

        public ISaldoDiarioRepository ISaldoDiarioRepository => new SaldoDiarioRepository(context);

        public IFechamentoRepository IFechamentoRepository => new FechamentoRepository(context);

        public ISaldoContaMensalRepository ISaldoMensalContaRepository => new SaldoContaMensalRepository(context);

        public IItemMovimentacaoMensalRepository IItemMovimentacaoMensalRepository => new ItemMovimentacaoMensalRepository(context);

        public void Dispose()
        {
            //context.Dispose();
        }
    }
}
