using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Conta
{
    public class DeleteContaCommand : IRequest
    {
        public int Id { get; set; }
    }
}
