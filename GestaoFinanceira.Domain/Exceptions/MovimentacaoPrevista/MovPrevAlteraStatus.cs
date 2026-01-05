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
        //public readonly int? Id;

        public MovPrevAlteraStatus(string descricaoItemMovimentacao, DateTime dataReferencia, StatusMovimentacaoPrevista status/*, int? _idMovimentacaoRealizada*/)
        {
            this.descricaoItemMovimentacao = descricaoItemMovimentacao;
            this.mesReferencia = dataReferencia.ToString("MM/yyyy");
            this.status = status.ObterDescricao();
            //this.Id = _idMovimentacaoRealizada;
        }

        public override string Message => $"Movimentação gravada com sucesso! \r\n A Movimentação Prevista para {this.descricaoItemMovimentacao} referente ao mês de {this.mesReferencia} teve seu status alterado para {this.status}";


    }
}
