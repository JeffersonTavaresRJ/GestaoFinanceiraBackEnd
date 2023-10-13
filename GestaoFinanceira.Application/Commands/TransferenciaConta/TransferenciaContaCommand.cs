using MediatR;
using System;

namespace GestaoFinanceira.Application.Commands.TransferenciaConta
{
    public class TransferenciaContaCommand : IRequest
    {
        public int IdConta { get; set; }
        public int IdContaDestino { get; set; }
        public DateTime DataMovimentacaoRealizada { get; set; }
        public double Valor { get; set; }

    }

}
