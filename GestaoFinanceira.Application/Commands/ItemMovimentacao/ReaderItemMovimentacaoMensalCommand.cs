using GestaoFinanceira.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Application.Commands.ItemMovimentacao
{
    public class ReaderItemMovimentacaoMensalCommand : IRequest<List<ItemMovimentacaoMensalDTO>>
    {
        public int IdUsuario { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}
