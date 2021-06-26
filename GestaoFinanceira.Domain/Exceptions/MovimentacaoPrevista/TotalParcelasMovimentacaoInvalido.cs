using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class TotalParcelasMovimentacaoInvalidoException : Exception
    {
        private int TotalParcelas;

        public TotalParcelasMovimentacaoInvalidoException(int totalParcelas)
        {
            TotalParcelas = totalParcelas;
        }

        public override string Message => $"O total de parcelas deve ser maior do que {TotalParcelas}";
    }
}
