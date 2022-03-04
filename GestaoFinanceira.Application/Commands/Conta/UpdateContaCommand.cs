using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Conta
{
    public class UpdateContaCommand : IRequest
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
    }
}
