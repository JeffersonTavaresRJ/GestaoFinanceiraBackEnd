using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class FormaPagamentoDomainService : GenericDomainService<FormaPagamento>, IFormaPagamentoDomainService
    {
        private readonly IUnitOfWork unitOfwork;

        public FormaPagamentoDomainService(IUnitOfWork unitOfwork):base(unitOfwork.IFormaPagamentoRepository)
        {
            this.unitOfwork = unitOfwork;
        }

        public override List<FormaPagamento> GetAll(int idUsuario)
        {
            return unitOfwork.IFormaPagamentoRepository.GetAll(idUsuario).ToList();
        }
    }
}
