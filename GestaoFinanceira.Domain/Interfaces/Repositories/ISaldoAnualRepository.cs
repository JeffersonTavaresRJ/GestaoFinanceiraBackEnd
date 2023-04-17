using GestaoFinanceira.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface ISaldoAnualRepository
    {
        Task<IEnumerable<SaldoAnual>> GetSaldoAnual(int idUsuario, int anoInicial, int anoFinal);
    }
}
