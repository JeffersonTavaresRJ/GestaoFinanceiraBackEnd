using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoRealizadaDomainService
    {
        MovimentacaoRealizada Add(MovimentacaoRealizada movimentacoesRealizada, out MovimentacaoPrevista movimentacoesPrevista);
        MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista);
        void Delete(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista);
        MovimentacaoRealizada GetId(int id);

    }
}
