using AutoMapper;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
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
                .AfterMap((scr, dest) => dest.Tipo = scr.Tipo.ToString())
                .AfterMap((scr, dest) => dest.TipoDescricao = ExtensionEnum.ObterDescricao((TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), dest.Tipo.ToString())));

            CreateMap<MovimentacaoPrevista, MovimentacaoPrevistaDTO>()
                .AfterMap((scr, dest) => dest.Status = scr.Status.ToString())
                .AfterMap((scr, dest) => dest.StatusDescricao = ExtensionEnum.ObterDescricao((StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), dest.Status.ToString())))
                .AfterMap((scr, dest) => dest.TipoPrioridade = scr.Movimentacao.TipoPrioridade.ToString())
                .AfterMap((scr, dest) => dest.TipoPrioridadeDescricao = ExtensionEnum.ObterDescricao((TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), scr.Movimentacao.TipoPrioridade.ToString())))
                .AfterMap((scr, dest) => dest.Observacao = scr.Movimentacao.Observacao)
                .AfterMap((scr, dest) => dest.DataReferencia = new DateTime(scr.DataReferencia.Year, scr.DataReferencia.Month, scr.DataReferencia.Day))
                .AfterMap((scr, dest) => dest.DataVencimento = new DateTime(scr.DataVencimento.Year, scr.DataVencimento.Month, scr.DataVencimento.Day))
                .AfterMap((scr, dest) => dest.Parcela = scr.NrParcelaTotal > 1 ? $"({scr.NrParcela}/{scr.NrParcelaTotal})": "");

            CreateMap<MovimentacaoRealizada, MovimentacaoRealizadaDTO>()
                .AfterMap((scr, dest) => dest.TipoPrioridade = scr.Movimentacao.TipoPrioridade.ToString())
                .AfterMap((scr, dest) => dest.TipoPrioridadeDescricao = ExtensionEnum.ObterDescricao((TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), scr.Movimentacao.TipoPrioridade.ToString())))
                .AfterMap((scr, dest) => dest.DataMovimentacaoRealizada = new DateTime(scr.DataMovimentacaoRealizada.Year, scr.DataMovimentacaoRealizada.Month, scr.DataMovimentacaoRealizada.Day))
                .AfterMap((scr, dest) => dest.DataReferencia = new DateTime(scr.DataReferencia.Year, scr.DataReferencia.Month, scr.DataReferencia.Day))
                .AfterMap((scr, dest) => dest.Observacao = scr.Movimentacao.Observacao);

            CreateMap<SaldoDiario, SaldoDiarioDTO>()
                .AfterMap((scr, dest) => dest.DataSaldo = new DateTime(scr.DataSaldo.Year, scr.DataSaldo.Month, scr.DataSaldo.Day));

        }
    }
}
