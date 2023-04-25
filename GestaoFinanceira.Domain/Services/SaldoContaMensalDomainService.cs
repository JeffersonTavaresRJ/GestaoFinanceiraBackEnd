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
    public class SaldoContaMensalDomainService : ISaldoContaMensalDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public SaldoContaMensalDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public  async Task<IEnumerable<SaldoContaMensal>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal)
        {
            return await unitOfWork.ISaldoMensalContaRepository.GetSaldoMensalConta(idUsuario, anoInicial, anoFinal);

        }

       

        //public List<SaldoMensalConta> GetSaldoMensalConta()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<List<SaldoMensalConta>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal)
        //{
        //    return Task.Run(() => {
        //        return unitOfWork.ISaldoMensalContaRepository.GetSaldoMensalConta(idUsuario, anoInicial, anoFinal).ToList();
        //    });
        //}
    }
}
