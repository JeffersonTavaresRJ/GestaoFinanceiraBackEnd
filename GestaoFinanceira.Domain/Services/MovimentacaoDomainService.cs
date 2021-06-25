using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class MovimentacaoDomainService : IMovimentacaoDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public MovimentacaoDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Movimentacao GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            return unitOfWork.IMovimentacaoRepository.GetByKey(idItemMovimentacao, dataReferencia);
        }
    }
}
