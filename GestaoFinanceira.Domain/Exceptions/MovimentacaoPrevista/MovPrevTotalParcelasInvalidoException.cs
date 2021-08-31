using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class MovPrevTotalParcelasInvalidoException : Exception
    {
        private int TotalParcelas;

        public MovPrevTotalParcelasInvalidoException(int totalParcelas)
        {
            TotalParcelas = totalParcelas;
        }

        public override string Message => $"O total de parcelas deve ser maior do que {TotalParcelas}";
    }
}
