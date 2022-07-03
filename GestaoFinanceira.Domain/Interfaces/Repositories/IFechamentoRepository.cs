using System;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IFechamentoRepository
    {
        void Executar(int idUsuario, DateTime dataReferencia);
    }
}
