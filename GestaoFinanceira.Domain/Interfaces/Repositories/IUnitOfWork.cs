using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();

        IUsuarioRepository IUsuarioRepository { get; }
        ICategoriaRepository ICategoriaRepository { get; }
        IContaRepository IContaRepository { get; }
        IFormaPagamentoRepository IFormaPagamentoRepository { get; }
        IItemMovimentacaoRepository IItemMovimentacaoRepository { get; }
        IMovimentacaoRepository IMovimentacaoRepository {get;}
        IMovimentacaoPrevistaRepository IMovimentacaoPrevistaRepository { get; }
        IMovimentacaoRealizadaRepository IMovimentacaoRealizadaRepository { get; }
        ISaldoDiarioRepository ISaldoDiarioRepository { get; }
        IFechamentoRepository IFechamentoRepository { get; }
        ISaldoAnualRepository ISaldoAnualRepository { get; }

    }
}
