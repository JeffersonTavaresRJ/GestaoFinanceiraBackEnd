using GestaoFinanceira.Application.Commands.FormaPagamento;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class FormaPagamentoApplicationService: IFormaPagamentoApplicationService
    {
        private readonly IMediator mediator;
        private readonly IFormaPagamentoCaching formaPagamentoCaching;

        public FormaPagamentoApplicationService(IMediator mediator, IFormaPagamentoCaching formaPagamentoCaching)
        {
            this.mediator = mediator;
            this.formaPagamentoCaching = formaPagamentoCaching;
        }

        public Task Add(CreateFormaPagamentoCommand command)
        {
            return mediator.Send(command);
        }

        public Task Update(UpdateFormaPagamentoCommand command)
        {
            return mediator.Send(command);
        }

        public Task Delete(DeleteFormaPagamentoCommand command)
        {
            return mediator.Send(command);
        }

        public FormaPagamentoDTO GetById(int id)
        {
            return formaPagamentoCaching.GetId(id);
        }

        public List<FormaPagamentoDTO> GetAll()
        {
            return formaPagamentoCaching.GetAll();
        }
    }
}
