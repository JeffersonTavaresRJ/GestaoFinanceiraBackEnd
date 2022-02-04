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
        public List<string> Messages;

        public MovPrevAlteraStatus(List<string> _messages)
        {
            this.Messages = _messages;
        }

        public MovPrevAlteraStatus(string descricaoItemMovimentacao, DateTime dataReferencia, StatusMovimentacaoPrevista status)
        {
            this.descricaoItemMovimentacao = descricaoItemMovimentacao;
            this.mesReferencia = dataReferencia.ToString("MM/yyyy");
            this.status = status.ObterDescricao();
        }

        public override string Message => $"Movimentação gravada com sucesso! \r\n A Movimentação Prevista para {this.descricaoItemMovimentacao} referente ao mês de {this.mesReferencia} teve seu status alterado para {this.status}";


    }
}
