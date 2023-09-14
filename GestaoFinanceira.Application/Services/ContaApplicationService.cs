using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Reports.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class ContaApplicationService : IContaApplicationService
    {

        private readonly IMediator mediator;
        private readonly IContaCaching contaCaching;

        public ContaApplicationService(IMediator mediator, IContaCaching contaCaching)
        {
            this.mediator = mediator;
            this.contaCaching = contaCaching;
        }

        public async Task Add(CreateContaCommand command)
        {
            await mediator.Send(command);
        }

        public async Task Update(UpdateContaCommand command)
        {
            await mediator.Send(command);
        }

        public async Task Delete(DeleteContaCommand command)
        {
            await mediator.Send(command);
        }

        public ContaDTO GetId(int id)
        {
            return contaCaching.GetId(id);
        }

        public List<ContaDTO> GetAll()
        {
            return contaCaching.GetAll();
        }

        public byte[] GetReport()
        {
            return ReportContas.Get(contaCaching.GetAll());
        }
    }
}
