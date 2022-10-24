using MediatR;
using System;

namespace GestaoFinanceira.Application.Commands.Fechamento
{
    public class CreateFechamentoCommand : IRequest
    {
        public DateTime DataReferencia { get; set; }
        public string Status { get; set; }
    }
}
