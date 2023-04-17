using GestaoFinanceira.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.SaldoAnual
{
    public class ReaderSaldoAnualPorContaCommand: IRequest<List<SaldoAnualPorContaDTO>>
    {
        public int IdUsuario { get; set; }
        public int Ano { get; set; }

    }
}
