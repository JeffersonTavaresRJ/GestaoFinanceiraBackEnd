using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IItemMovimentacaoMensalRepository
    {
        Task<IEnumerable<ItemMovimentacaoMensal>> GetItemMovimentacaoMensal(int idUsuario, DateTime dataIncial, DateTime dataFinal);
    }
}

