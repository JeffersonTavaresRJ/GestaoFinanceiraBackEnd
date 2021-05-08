using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class ContaDomainService : GenericDomainService<Conta>, IContaDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public ContaDomainService(IUnitOfWork unitOfWork):base(unitOfWork.IContaRepository)
        {
            this.unitOfWork = unitOfWork;
        }       
        

        public override List<Conta> GetAll(int idUsuario)
        {
            return this.unitOfWork.IContaRepository.GetAll(idUsuario).ToList();
        }
    }
}
