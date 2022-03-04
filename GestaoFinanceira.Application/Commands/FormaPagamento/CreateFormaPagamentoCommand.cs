using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.FormaPagamento
{
    public class CreateFormaPagamentoCommand : IRequest
    {
        public string Descricao { get; set; }
    }
}
