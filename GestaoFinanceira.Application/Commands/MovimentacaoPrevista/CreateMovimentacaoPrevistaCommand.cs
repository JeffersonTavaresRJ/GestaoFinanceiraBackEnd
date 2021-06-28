using GestaoFinanceira.Application.Commands.Movimentacao;
using GestaoFinanceira.Domain.Models.Enuns;
using MediatR;
using System;
using System.ComponentModel;

namespace GestaoFinanceira.Application.Commands.MovimentacaoPrevista
{
    public class CreateMovimentacaoPrevistaCommand : MovimentacaoCommand, IRequest
    {
        public DateTime DataVencimento { get; set; }
        public double Valor { get; set; }
        public string Status { get; set; }
        public int IdFormaPagamento { get; set; }
        public string TipoRecorrencia { get; set; }
        public int QtdeParcelas { get; set; }
    }
    
}
