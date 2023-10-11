using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoRealizadaDomainService
    {
        MovimentacaoRealizada Add(MovimentacaoRealizada movimentacoesRealizada, out MovimentacaoPrevista movimentacoesPrevista);
        List<MovimentacaoRealizada> ExecutarTransferencia(TransferenciaContas transferenciaConta);
        MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista);
        void Delete(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista);
        MovimentacaoRealizada GetId(int id);
        List<MovimentacaoRealizada> GetByUsuario(int idUsuario, DateTime dataReferencia);

    }
}
