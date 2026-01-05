using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class DeleteMovimentacaoRealizadaCommand : IRequest
    {
        public int Id { get; set; }
    }
}
