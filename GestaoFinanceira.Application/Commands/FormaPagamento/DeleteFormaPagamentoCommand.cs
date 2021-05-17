using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.FormaPagamento
{
    public class DeleteFormaPagamentoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
