using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.Movimentacao
{
    public class MovDataReferenciaException : Exception
    {
        private string DataVencimento;
        private string DataReferencia;
        private string Campo;

        public MovDataReferenciaException(string campo, DateTime dataVencimento, DateTime dataReferencia)
        {
            DataVencimento = dataVencimento.ToString("dd/MM/yyyy");
            DataReferencia = dataReferencia.ToString("MM/yyyy");
            Campo = campo;
        }

        public override string Message => $"A {Campo} {DataVencimento} possui o mês/ano diferente do período de referência da movimentação: {DataReferencia}";
    }
}
