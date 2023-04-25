using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Commands.SaldoMensalConta;
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


    public class MovimentacaoRealizadaQueryHandler :   IRequestHandler<ReaderSaldoMensalPorContaCommand, List<SaldoContaMensalDTO>>,
                                                       IRequestHandler<ReaderSaldoAnualPorContaCommand, List<SaldoContaAnualDTO>>
    {
        private readonly ISaldoContaMensalDomainService SaldoContaMensalDomainService;
        private readonly IMapper mapper;


        public MovimentacaoRealizadaQueryHandler(  ISaldoContaMensalDomainService SaldoContaMensalDomainService,
                                                   IMapper mapper)
        {
            this.SaldoContaMensalDomainService= SaldoContaMensalDomainService;
            this.mapper = mapper; 
        }

        public async Task<List<SaldoContaMensalDTO>> Handle(ReaderSaldoMensalPorContaCommand request, CancellationToken cancellationToken)
        {
            List<SaldoContaMensal> lista = (List <SaldoContaMensal>) await SaldoContaMensalDomainService.GetSaldoMensalConta(request.IdUsuario, request.Ano, request.Ano);
            List<SaldoContaMensalDTO> result = new List<SaldoContaMensalDTO>();

            foreach (SaldoContaMensal item in lista)
            {
                result.Add(mapper.Map<SaldoContaMensalDTO>(item));
            }

            return result;
        }

        public async Task<List<SaldoContaAnualDTO>> Handle(ReaderSaldoAnualPorContaCommand request, CancellationToken cancellationToken)
        {
            List<SaldoContaAnualDTO> lista = new List<SaldoContaAnualDTO>();

            foreach (SaldoContaMensal item in await SaldoContaMensalDomainService.GetSaldoMensalConta(request.IdUsuario, request.AnoInicial, request.AnoFinal))
            {
                lista.Add(mapper.Map<SaldoContaAnualDTO>(item));
            }

            return lista;
        }
    }
}