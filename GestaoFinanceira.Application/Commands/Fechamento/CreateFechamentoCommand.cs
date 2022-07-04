using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Fechamento
{
    public class CreateFechamentoCommand : IRequest
    {
        public int IdUsuario { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
