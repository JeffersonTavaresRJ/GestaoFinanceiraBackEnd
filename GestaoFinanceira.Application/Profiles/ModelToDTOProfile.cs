using AutoMapper;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using GestaoFinanceira.Infra.CrossCutting.Security;
using System;

namespace GestaoFinanceira.Application.Profiles
{
    public class ModelToDTOProfile : Profile
    {
        public ModelToDTOProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();

            CreateMap<Categoria, CategoriaDTO>();
           
            CreateMap<Conta, ContaDTO>();

            CreateMap<FormaPagamento, FormaPagamentoDTO>();

            CreateMap<ItemMovimentacao, ItemMovimentacaoDTO>()
                .AfterMap((scr, dest) => dest.Categoria = new CategoriaDTO())
                .AfterMap((scr, dest) => dest.Categoria.Id = scr.IdCategoria)
                .AfterMap((scr, dest) => dest.Tipo = scr.Tipo.ToString())
                .AfterMap((scr, dest) => dest.TipoDescricao = ExtensionEnum.ObterDescricao((TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), dest.Tipo.ToString())));

            CreateMap<MovimentacaoPrevista, MovimentacaoPrevistaDTO>()
                .AfterMap((scr, dest) => dest.ItemMovimentacao = new ItemMovimentacaoDTO())
                .AfterMap((scr, dest) => dest.ItemMovimentacao.Id = scr.IdItemMovimentacao)
                .AfterMap((scr, dest) => dest.FormaPagamento = new FormaPagamentoDTO())
                .AfterMap((scr, dest) => dest.FormaPagamento.Id = scr.IdFormaPagamento)
                .AfterMap((scr, dest) => dest.FormaPagamento.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((src, dest) => dest.ItemMovimentacao.Categoria = new CategoriaDTO())
                .AfterMap((scr, dest) => dest.ItemMovimentacao.Categoria.Id = scr.Movimentacao.ItemMovimentacao.IdCategoria)
                .AfterMap((scr, dest) => dest.ItemMovimentacao.Categoria.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((scr, dest) => dest.Status = scr.Status.ToString())
                .AfterMap((scr, dest) => dest.StatusDescricao = ExtensionEnum.ObterDescricao((StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), dest.Status.ToString())))
                .AfterMap((scr, dest) => dest.TipoPrioridade = scr.Movimentacao.TipoPrioridade.ToString())
                .AfterMap((scr, dest) => dest.TipoPrioridadeDescricao = ExtensionEnum.ObterDescricao((TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), scr.Movimentacao.TipoPrioridade.ToString())))
                .AfterMap((scr, dest) => dest.Observacao = scr.Movimentacao.Observacao)
                .AfterMap((scr, dest) => dest.DataReferencia = new DateTime(scr.DataReferencia.Year, scr.DataReferencia.Month, scr.DataReferencia.Day,0,0,0))
                .AfterMap((scr, dest) => dest.DataVencimento = new DateTime(scr.DataVencimento.Year, scr.DataVencimento.Month, scr.DataVencimento.Day,0,0,0))
                .AfterMap((scr, dest) => dest.Parcela = scr.NrParcelaTotal > 1 ? $"({scr.NrParcela}/{scr.NrParcelaTotal})": "");

            CreateMap<MovimentacaoRealizada, MovimentacaoRealizadaDTO>()
                .AfterMap((scr, dest) => dest.ItemMovimentacao = new ItemMovimentacaoDTO())
                .AfterMap((scr, dest) => dest.ItemMovimentacao.Id = scr.IdItemMovimentacao)
                .AfterMap((scr, dest) => dest.FormaPagamento = new FormaPagamentoDTO())
                .AfterMap((scr, dest) => dest.FormaPagamento.Id = scr.IdFormaPagamento)
                .AfterMap((scr, dest) => dest.FormaPagamento.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((scr, dest) => dest.Conta = new ContaDTO())
                .AfterMap((scr, dest) => dest.Conta.Id = scr.IdConta)
                .AfterMap((scr, dest) => dest.Conta.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((scr, dest) => dest.TipoPrioridade = scr.Movimentacao.TipoPrioridade.ToString())
                .AfterMap((scr, dest) => dest.TipoPrioridadeDescricao = ExtensionEnum.ObterDescricao((TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), scr.Movimentacao.TipoPrioridade.ToString())))
                .AfterMap((scr, dest) => dest.DataMovimentacaoRealizada = new DateTime(scr.DataMovimentacaoRealizada.Year, scr.DataMovimentacaoRealizada.Month, scr.DataMovimentacaoRealizada.Day,0,0,0))
                .AfterMap((scr, dest) => dest.DataReferencia = new DateTime(scr.DataReferencia.Year, scr.DataReferencia.Month, scr.DataReferencia.Day,0,0,0))
                .AfterMap((scr, dest) => dest.Observacao = scr.Movimentacao.Observacao);

            CreateMap<SaldoDiario, SaldoDiarioDTO>()
                .AfterMap((scr, dest) => dest.DataSaldo = new DateTime(scr.DataSaldo.Year, scr.DataSaldo.Month, scr.DataSaldo.Day,0,0,0));

            CreateMap<SaldoAnual, SaldoAnualPorContaDTO>();

            CreateMap<SaldoAnual, SaldoAnualPorPeriodoDTO>()
                .AfterMap((scr, dest) => dest.IdConta = scr.IdConta)
                .AfterMap((scr, dest) => dest.DescricaoConta = scr.DescricaoConta)
                .AfterMap((scr, dest) => dest.Ano = scr.Ano)
                .AfterMap((scr, dest) => dest.Saldo = scr.Dezembro);
        }
    }
}
