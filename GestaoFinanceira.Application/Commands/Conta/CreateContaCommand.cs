using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Conta
{
    public class CreateContaCommand : IRequest
    {
        public string Descricao { get; set; }
    }
}
