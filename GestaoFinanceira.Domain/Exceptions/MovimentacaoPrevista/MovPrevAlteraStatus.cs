using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class MovPrevAlteraStatus : Exception
    {
        private List<string> descricaoItensMovimentacao;
        private string mesReferencia;
        private string status;
        private string mensagem;

        public MovPrevAlteraStatus(List<string> descricaoItensMovimentacao, DateTime dataReferencia, StatusMovimentacaoPrevista status)
        {
            this.descricaoItensMovimentacao = descricaoItensMovimentacao;
            this.mesReferencia = dataReferencia.ToString("MM/yyyy");
            this.status = status.ObterDescricao();

            this.mensagem = $"Movimentação gravada com sucesso! " +
            $"\r\n Para o mês de {this.mesReferencia}, a(s) Movimentação(ões) Prevista(s) abaixo tiveram seu status alterado para {this.status}:";

            foreach (var item in this.descricaoItensMovimentacao)
            {
                this.mensagem += $"\r\n{item}";
            }
        }

        public override string Message => this.mensagem;


    }
}
