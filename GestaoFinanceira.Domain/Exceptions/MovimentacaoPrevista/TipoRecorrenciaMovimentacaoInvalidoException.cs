using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class TipoRecorrenciaMovimentacaoInvalidoException : Exception
    {
        public override string Message => "A recorrência é diferente de 'N - Nenhum', 'M-Mensal' e 'P-Parcelado'";
    }
}
