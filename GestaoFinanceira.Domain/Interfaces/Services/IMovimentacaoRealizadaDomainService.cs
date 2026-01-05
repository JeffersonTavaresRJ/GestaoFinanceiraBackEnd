using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoRealizadaDomainService
    {
        MovimentacaoRealizada Add(MovimentacaoRealizada movimentacoesRealizada, out List<MovimentacaoPrevista> movimentacoesPrevistas, string statusMovimentacaoPrevista=null);
        List<MovimentacaoRealizada> ExecutarTransferencia(TransferenciaContas transferenciaConta);
        MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out List<MovimentacaoPrevista> movimentacoesPrevistas, string statusMovimentacaoPrevista=null);
        void Delete(MovimentacaoRealizada movimentacaoRealizada, out List<MovimentacaoPrevista> movimentacoesPrevistas);
        MovimentacaoRealizada GetId(int id);
        List<MovimentacaoRealizada> GetByUsuario(int idUsuario, DateTime dataReferencia);

    }
}
