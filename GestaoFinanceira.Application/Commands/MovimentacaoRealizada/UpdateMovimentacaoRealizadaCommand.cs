using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class UpdateMovimentacaoRealizadaCommand : IRequest
    {
        public int Id { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public double Valor { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdConta { get; set; }
    }
}
