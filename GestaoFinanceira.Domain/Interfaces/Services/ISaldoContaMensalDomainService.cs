using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface ISaldoContaMensalDomainService
    {
       Task<IEnumerable<SaldoContaMensal>>GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal);
    }
}
