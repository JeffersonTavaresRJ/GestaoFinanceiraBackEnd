using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class MovPrevAlteraStatus : Exception
    {
        private string descricaoItemMovimentacao;
        private string mesReferencia;
        private string status;

        public MovPrevAlteraStatus(string descricaoItemMovimentacao, DateTime dataReferencia, StatusMovimentacaoPrevista status)
        {
            this.descricaoItemMovimentacao = descricaoItemMovimentacao;
            this.mesReferencia = dataReferencia.ToString("MM/yyyy");
            this.status = status.ObterDescricao();
        }

        public override string Message => $"O {this.descricaoItemMovimentacao} referente ao mês de {this.mesReferencia} teve seu status alterado para {this.status}";


    }
}
