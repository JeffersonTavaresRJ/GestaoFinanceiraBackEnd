using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class ReaderMovimentacaoMensalPorConta
    {
        public List<int> IdContas { get; set; }
        public DateTime DataReferencia { get; set; }
        public int totalMeses { get; set; }
    }
}
