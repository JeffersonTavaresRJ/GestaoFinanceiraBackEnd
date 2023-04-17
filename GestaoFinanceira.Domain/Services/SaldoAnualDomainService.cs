using GestaoFinanceira.Domain.Interfaces.Repositories;
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
    public class SaldoAnualDomainService : ISaldoAnualDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public SaldoAnualDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public  async Task<IEnumerable<SaldoAnual>> GetSaldoAnual(int idUsuario, int anoInicial, int anoFinal)
        {
            return await unitOfWork.ISaldoAnualRepository.GetSaldoAnual(idUsuario, anoInicial, anoFinal);

        }

       

        //public List<SaldoAnual> GetSaldoAnual()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<SaldoAnual>> GetSaldoAnual(int idUsuario, int anoInicial, int anoFinal)
        //{
        //    return Task.Run(() => {
        //        return unitOfWork.ISaldoAnualRepository.GetSaldoAnual(idUsuario, anoInicial, anoFinal).ToList();
        //    });
        //}
    }
}
