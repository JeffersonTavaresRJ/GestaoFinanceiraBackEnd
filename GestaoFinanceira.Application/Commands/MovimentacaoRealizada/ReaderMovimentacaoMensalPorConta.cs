using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.MovimentacaoRealizada
{
    public class ReaderMovimentacaoMensalPorConta
    {
        public List<int> IdContas { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
