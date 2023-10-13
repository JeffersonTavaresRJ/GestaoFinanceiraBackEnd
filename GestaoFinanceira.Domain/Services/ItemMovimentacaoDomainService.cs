using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class ItemMovimentacaoDomainService: GenericDomainService<ItemMovimentacao>, IItemMovimentacaoDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemMovimentacaoDomainService(IUnitOfWork unitOfWork):base(unitOfWork.IItemMovimentacaoRepository)
        {
            this.unitOfWork = unitOfWork;
        }

        public override List<ItemMovimentacao> GetAll(int idUsuario)
        {
           return unitOfWork.IItemMovimentacaoRepository.GetAll(idUsuario).ToList();
        }
    }
}
