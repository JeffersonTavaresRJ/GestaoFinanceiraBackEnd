using GestaoFinanceira.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface ISaldoContaMensalRepository
    {
        Task<IEnumerable<SaldoContaMensal>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal);
    }
}
