using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Commands.SaldoAnual;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoRealizada;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{


    public class MovimentacaoRealizadaQueryHandler :   IRequestHandler<ReaderSaldoAnualPorContaCommand, List<SaldoAnualPorContaDTO>>,
                                                       IRequestHandler<ReaderSaldoAnualPorPeriodoCommand, List<SaldoAnualPorPeriodoDTO>>
    {
        private readonly ISaldoAnualDomainService saldoAnualDomainService;
        private readonly IMapper mapper;


        public MovimentacaoRealizadaQueryHandler(  ISaldoAnualDomainService saldoAnualDomainService,
                                                   IMapper mapper)
        {
            this.saldoAnualDomainService= saldoAnualDomainService;
            this.mapper = mapper; 
        }

        public async Task<List<SaldoAnualPorContaDTO>> Handle(ReaderSaldoAnualPorContaCommand request, CancellationToken cancellationToken)
        {
            List<SaldoAnual> lista = (List <SaldoAnual>) await saldoAnualDomainService.GetSaldoAnual(request.IdUsuario, request.Ano, request.Ano);
            List<SaldoAnualPorContaDTO> result = new List<SaldoAnualPorContaDTO>();

            foreach (SaldoAnual item in lista)
            {
                result.Add(mapper.Map<SaldoAnualPorContaDTO>(item));
            }

            return result;
        }

        public async Task<List<SaldoAnualPorPeriodoDTO>> Handle(ReaderSaldoAnualPorPeriodoCommand request, CancellationToken cancellationToken)
        {
            List<SaldoAnualPorPeriodoDTO> lista = new List<SaldoAnualPorPeriodoDTO>();

            foreach (SaldoAnual item in await saldoAnualDomainService.GetSaldoAnual(request.IdUsuario, request.AnoInicial, request.AnoFinal))
            {
                lista.Add(mapper.Map<SaldoAnualPorPeriodoDTO>(item));
            }

            return lista;
        }
    }
}