using GestaoFinanceira.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.SaldoAnual
{
    public class ReaderSaldoAnualPorPeriodoCommand: IRequest<List<SaldoAnualPorPeriodoDTO>>
    {
        public int IdUsuario { get; set; }
        public int AnoInicial { get; set; }
        public int AnoFinal { get; set; }

    }
}
