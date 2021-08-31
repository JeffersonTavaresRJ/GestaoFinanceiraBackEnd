using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class MovPrevExisteException : Exception
    {
        private string descricaoItemMovimentacao;
        private string mesReferencia;

        public MovPrevExisteException(string descricaoItemMovimentacao, DateTime dataReferencia)
        {
            this.descricaoItemMovimentacao = descricaoItemMovimentacao;
            this.mesReferencia = dataReferencia.ToString("MM/yyyy");
        }

        public override string Message => $"O item de movimentação {this.descricaoItemMovimentacao} já está cadastrado para o mês de {this.mesReferencia}";
    }
}
