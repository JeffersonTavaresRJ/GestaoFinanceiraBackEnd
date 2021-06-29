using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class StatusMovimentacaoInvalidoException :Exception
    {
        private string DescricaoItemMovimentacao;
        private DateTime DataReferencia;
        private string Status;

        public StatusMovimentacaoInvalidoException(string descricaoItemMovimentacao, 
                                                   DateTime dataReferencia,
                                                   StatusMovimentacaoPrevista statusMovimentacaoPrevista)
        {
            DataReferencia = dataReferencia;
            DescricaoItemMovimentacao = descricaoItemMovimentacao;

            if (!statusMovimentacaoPrevista.Equals(StatusMovimentacaoPrevista.Q))
            {
                Status = "quitado";
            }
            else
            {
                Status = "em aberto";
            }
            
        }

        public override string Message 
            => $"O item '{DescricaoItemMovimentacao}' para o mês de '{DataReferencia.ToString("MM/yyyy")}' encontra-se {Status}.\n Status Inválido.";

    }
}
