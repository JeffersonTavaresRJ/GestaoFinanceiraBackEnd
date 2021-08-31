using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoRealizadaDomainService
    {
        void Add(List<MovimentacaoRealizada> movimentacoesRealizadas);
        void Update(MovimentacaoRealizada movimentacaoRealizada);
        void Delete(MovimentacaoRealizada movimentacaoRealizada);
        MovimentacaoRealizada GetId(int id);

    }
}
