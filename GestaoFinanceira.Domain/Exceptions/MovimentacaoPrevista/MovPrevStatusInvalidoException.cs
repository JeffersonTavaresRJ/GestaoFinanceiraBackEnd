using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista
{
    public class MovPrevStatusInvalidoException :Exception
    {
        private string DescricaoItemMovimentacao;
        private DateTime DataReferencia;
        private string Status;
        private string Mensagem;

        public MovPrevStatusInvalidoException(string descricaoItemMovimentacao, 
                                                   DateTime dataReferencia,
                                                   StatusMovimentacaoPrevista statusMovimentacaoPrevista)
        {
            DataReferencia = dataReferencia;
            DescricaoItemMovimentacao = descricaoItemMovimentacao;

            if (statusMovimentacaoPrevista.Equals(StatusMovimentacaoPrevista.Q) || 
                statusMovimentacaoPrevista.Equals(StatusMovimentacaoPrevista.A))
            {
                Status = statusMovimentacaoPrevista.ObterDescricao();
                Mensagem = $"O item '{DescricaoItemMovimentacao}' para o mês de '{DataReferencia.ToString("MM/yyyy")}' encontra-se {Status}.\n Status Inválido.";
            }
            else
            {
                Mensagem= "Status Inválido.";
            }
            
        }

        public override string Message => Mensagem;
    }
}
