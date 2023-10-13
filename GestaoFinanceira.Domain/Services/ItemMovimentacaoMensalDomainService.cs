using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Services
{
    public class ItemMovimentacaoMensalDomainService : IItemMovimentacaoMensalDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemMovimentacaoMensalDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ItemMovimentacaoMensal>> GetItemMovimentacaoMensal(int idUsuario, DateTime dataIncial, DateTime dataFinal)
        {
            return await unitOfWork.IItemMovimentacaoMensalRepository.GetItemMovimentacaoMensal(idUsuario, dataIncial, dataFinal);
        }
     
    }
}
