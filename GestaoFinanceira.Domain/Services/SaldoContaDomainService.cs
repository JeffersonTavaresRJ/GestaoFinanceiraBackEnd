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
    public class SaldoContaDomainService : ISaldoContaDomainService
    {
        private readonly IUnitOfWork unitOfWork;

        public SaldoContaDomainService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SaldoContaAnual>> GetSaldoAnualConta(int idUsuario, int anoInicial, int anoFinal)
        {
            return await unitOfWork.ISaldoContaRepository.GetSaldoAnualConta(idUsuario, anoInicial, anoFinal);
        }

        public  async Task<IEnumerable<SaldoContaMensal>> GetSaldoMensalConta(int idUsuario, int anoInicial, int anoFinal)
        {
            return await unitOfWork.ISaldoContaRepository.GetSaldoMensalConta(idUsuario, anoInicial, anoFinal);

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
