using GestaoFinanceira.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.SaldoMensalConta
{
    public class ReaderSaldoMensalPorContaCommand: IRequest<List<SaldoContaMensalDTO>>
    {
        public int IdUsuario { get; set; }
        public int Ano { get; set; }

    }
}
