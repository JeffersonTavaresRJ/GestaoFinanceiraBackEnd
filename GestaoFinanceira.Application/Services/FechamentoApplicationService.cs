using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Interfaces.Services;
using System;

namespace GestaoFinanceira.Application.Services
{
    public class FechamentoApplicationService : IFechamentoApplicationService
    {
        protected IFechamentoDomainService fechamentoDomainService;

        public FechamentoApplicationService(IFechamentoDomainService fechamentoDomainService)
        {
            this.fechamentoDomainService = fechamentoDomainService;
        }

        public void Executar(int idUsuario, DateTime dataReferencia)
        {
            this.fechamentoDomainService.Executar(idUsuario, dataReferencia);
        }
    }
}
