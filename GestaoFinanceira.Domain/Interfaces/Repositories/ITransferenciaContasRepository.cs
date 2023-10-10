using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface ITransferenciaContasRepository
    {
        IEnumerable<dynamic> Executar(TransferenciaContas transfereConta);
    }
}
