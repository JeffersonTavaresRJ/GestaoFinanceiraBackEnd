using GestaoFinanceira.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface ISaldoContaRepository
    {
        Task<IEnumerable<SaldoContaMensal>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal);
        Task<IEnumerable<SaldoContaAnual>> GetSaldoAnualConta(int idUsuario, int anoInicial, int anoFinal);
    }
}
