using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.ItemMovimentacao
{
    public class DeleteItemMovimentacaoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
