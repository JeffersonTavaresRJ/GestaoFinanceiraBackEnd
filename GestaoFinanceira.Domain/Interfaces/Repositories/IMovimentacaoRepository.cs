using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IMovimentacaoRepository : IGenericWriteRepository<Movimentacao>
    {
        Movimentacao GetByKey(int idItMov, DateTime dataReferencia);
    }
}
