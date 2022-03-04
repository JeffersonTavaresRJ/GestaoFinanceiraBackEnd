using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoRealizadaDomainService
    {
        List<MovimentacaoRealizada> Add(List<MovimentacaoRealizada> movimentacoesRealizadas, out List<MovimentacaoPrevista> movimentacoesPrevistas);
        MovimentacaoRealizada Update(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista);
        void Delete(MovimentacaoRealizada movimentacaoRealizada, out MovimentacaoPrevista movimentacaoPrevista);
        MovimentacaoRealizada GetId(int id);

    }
}
