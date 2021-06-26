using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class StatusMovimentacaoInvalidoException :Exception
    {
        private string DescricaoItemMovimentacao;
        private DateTime DataReferencia;

        public StatusMovimentacaoInvalidoException(string descricaoItemMovimentacao, DateTime DataReferencia)
        {
            this.DataReferencia = DataReferencia;
            this.DescricaoItemMovimentacao = descricaoItemMovimentacao;
        }

        public override string Message 
            => $"O item '{DescricaoItemMovimentacao}' para o mês de '{DataReferencia.ToString("MM/yyyy")}' encontra-se quitado.\n Status Inválido.";

    }
}
