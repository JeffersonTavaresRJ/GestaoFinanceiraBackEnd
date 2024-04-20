using GestaoFinanceira.Application.Commands.Fechamento;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class FechamentoApplicationService : IFechamentoApplicationService
    {
        private readonly IMediator mediator;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;
        private readonly IFechamentoDomainService fechamentoDomainService;

        public FechamentoApplicationService(IMediator mediator, ISaldoDiarioCaching saldoDiarioCaching, IFechamentoDomainService fechamentoDomainService)
        {
            this.mediator = mediator;
            this.saldoDiarioCaching = saldoDiarioCaching;
            this.fechamentoDomainService = fechamentoDomainService;
        }

        public async Task Executar(CreateFechamentoCommand fechamentoCreateCommand)
        {
            await mediator.Send(fechamentoCreateCommand);
        }

        public List<FechamentoMensalDTO> GetAll()
        {
            
            return this.fechamentoDomainService.GetAll();

        }
    }
}
