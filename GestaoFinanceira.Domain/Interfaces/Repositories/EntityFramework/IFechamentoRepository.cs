using System;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IFechamentoRepository
    {
        void Executar(int idUsuario, DateTime dataReferencia, string status);
    }
}
