using System;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IFechamentoDomainService
    {
        void Executar(int idUsuario, DateTime dataReferencia);
    }
}
