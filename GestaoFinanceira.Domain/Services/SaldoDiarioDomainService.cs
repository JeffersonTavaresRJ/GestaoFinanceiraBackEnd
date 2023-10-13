using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.Services
{
    public class SaldoDiarioDomainService : ISaldoDiarioDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public SaldoDiarioDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public SaldoDiario GetByKey(int idConta, DateTime dataSaldo)
        {
            return unitOfWork.ISaldoDiarioRepository.GetByKey(idConta, dataSaldo);
        }

        public List<SaldoDiario> GetByPeriodo(int idUsuario, DateTime dataIni, DateTime dataFim)
        {
            return unitOfWork.ISaldoDiarioRepository.GetByPeriodo(idUsuario, dataIni, dataFim).ToList();
        }

        public List<SaldoDiario> GetBySaldosDiario(int idConta, DateTime dataSaldo)
        {
            return unitOfWork.ISaldoDiarioRepository.GetBySaldosDiario(idConta, dataSaldo).ToList();
        }
    }
}
