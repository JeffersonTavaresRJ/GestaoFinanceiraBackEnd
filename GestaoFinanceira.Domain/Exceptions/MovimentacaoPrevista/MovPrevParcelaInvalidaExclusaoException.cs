using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class MovPrevParcelaInvalidaExclusaoException: Exception
    {
        private int _parcela;
        private int _parcelaTotal;
        public MovPrevParcelaInvalidaExclusaoException(int parcela, int parcelaTotal)
        {
            this._parcela = parcela;
            this._parcelaTotal = parcelaTotal;
        }

        public override string Message => $"A {_parcela}ª parcela é inválida para exclusão. \r\n Somente é permitida a exclusão da 1ª ou última ({_parcelaTotal}ª parcela) desta Movimentação";
    }
}
