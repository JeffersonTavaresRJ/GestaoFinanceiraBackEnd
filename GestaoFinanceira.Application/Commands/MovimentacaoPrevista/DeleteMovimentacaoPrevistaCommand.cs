using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.MovimentacaoPrevista
{
    public class DeleteMovimentacaoPrevistaCommand : IRequest
    {

        public int Id { get; set; }

    }
}
