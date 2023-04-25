using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.ItemMovimentacao;
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
                                                       IRequestHandler<ReaderSaldoAnualPorContaCommand, List<SaldoContaAnualDTO>>,
                                                       IRequestHandler<ReaderItemMovimentacaoMensalCommand, List<ItemMovimentacaoMensalDTO>>
    {
        private readonly ISaldoContaMensalDomainService saldoContaMensalDomainService;
        private readonly IItemMovimentacaoMensalDomainService itemMovimentacaoMensalDomainService;
        private readonly IMapper mapper;


        public MovimentacaoRealizadaQueryHandler(  ISaldoContaMensalDomainService saldoContaMensalDomainService,
                                                   IItemMovimentacaoMensalDomainService itemMovimentacaoMensalDomainService,
                                                   IMapper mapper)
        {
            this.saldoContaMensalDomainService= saldoContaMensalDomainService;
            this.itemMovimentacaoMensalDomainService = itemMovimentacaoMensalDomainService;
            this.mapper = mapper; 
        }

        public async Task<List<SaldoContaMensalDTO>> Handle(ReaderSaldoMensalPorContaCommand request, CancellationToken cancellationToken)
        {
            List<SaldoContaMensal> lista = (List <SaldoContaMensal>) await saldoContaMensalDomainService.GetSaldoMensalConta(request.IdUsuario, request.Ano, request.Ano);
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

            foreach (SaldoContaMensal item in await saldoContaMensalDomainService.GetSaldoMensalConta(request.IdUsuario, request.AnoInicial, request.AnoFinal))
            {
                lista.Add(mapper.Map<SaldoContaAnualDTO>(item));
            }

            return lista;
        }

        public async Task<List<ItemMovimentacaoMensalDTO>> Handle(ReaderItemMovimentacaoMensalCommand request, CancellationToken cancellationToken)
        {
            List<ItemMovimentacaoMensalDTO> lista = new List<ItemMovimentacaoMensalDTO>();

            foreach (ItemMovimentacaoMensal item in await itemMovimentacaoMensalDomainService.GetItemMovimentacaoMensal(request.IdUsuario, request.DataInicial, request.DataFinal))
            {
                lista.Add(mapper.Map<ItemMovimentacaoMensalDTO>(item));
            }

            return lista;
        }
    }
}