using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IMovimentacaoParceladaRepository
    {
        void Add(MovimentacaoParcelada entity);
        void Delete(MovimentacaoParcelada entity);
        void Delete(int idUsuario);
        void Dispose();
    }
}
