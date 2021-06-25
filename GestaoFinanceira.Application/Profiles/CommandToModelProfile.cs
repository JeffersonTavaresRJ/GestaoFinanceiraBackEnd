using AutoMapper;
using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Application.Commands.FormaPagamento;
using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Application.Commands.Movimentacao;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using System;

namespace GestaoFinanceira.Application.Profiles
{
    public class CommandToModelProfile : Profile
    {
        public CommandToModelProfile()
        {
            #region Usuario 
            CreateMap<CreateUsuarioCommand, Usuario>();

            CreateMap<UpdateUsuarioCommand, Usuario>();

            CreateMap<DeleteUsuarioCommand, Usuario>();
            #endregion

            #region Categoria
            CreateMap<CreateCategoriaCommand, Categoria>()
                .AfterMap((src, dest) => dest.Status = true);
            CreateMap<UpdateCategoriaCommand, Categoria>();
            CreateMap<DeleteCategoriaCommand, Categoria>();
            
            #endregion

            #region Conta
            CreateMap<CreateContaCommand, Conta>()
                .AfterMap((src, dest)=>dest.Status = true);
            CreateMap<UpdateContaCommand, Conta>();
            CreateMap<DeleteContaCommand, Conta>();
            #endregion

            #region FormaPagamento
            CreateMap<CreateFormaPagamentoCommand, FormaPagamento>()
                .AfterMap((src, dest)=>dest.Status = true);
            CreateMap<UpdateFormaPagamentoCommand, FormaPagamento>();
            CreateMap<DeleteFormaPagamentoCommand, FormaPagamento>();
            #endregion

            #region ItemMovimentacao
            CreateMap<CreateItemMovimentacaoCommand, ItemMovimentacao>()
                .AfterMap((src, dest) => dest.Status = true)
                .AfterMap((src, dest) => dest.Tipo = (TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), src.Tipo));

            CreateMap<UpdateItemMovimentacaoCommand, ItemMovimentacao>()
                .AfterMap((src, dest) => dest.Tipo = (TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), src.Tipo));

            CreateMap<DeleteItemMovimentacaoCommand, ItemMovimentacao>();
            #endregion

            #region Movimentacao
            CreateMap<CreateMovimentacaoPrevistaCommand, Movimentacao>()
                .AfterMap((src, dest) => dest.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade));
            CreateMap<UpdateMovimentacaoPrevistaCommand, Movimentacao>()
                .AfterMap((src, dest) => dest.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade));

            #endregion

            #region MovimentacaoPrevista
            CreateMap<CreateMovimentacaoPrevistaCommand, MovimentacaoPrevista>()
                .AfterMap((src, dest) => dest.Status = (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), src.Status))
                .AfterMap((src, dest) => dest.Movimentacao = new Movimentacao())
                .AfterMap((src, dest)=>dest.Movimentacao.IdItemMovimentacao = src.IdItemMovimentacao)
                .AfterMap((src, dest)=>dest.Movimentacao.DataReferencia = src.DataReferencia)
                .AfterMap((src, dest)=>dest.Movimentacao.Observacao = src.Observacao)
                .AfterMap((src, dest)=>dest.Movimentacao.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade));

            CreateMap<UpdateMovimentacaoPrevistaCommand, MovimentacaoPrevista>()
                .AfterMap((src, dest) => dest.Status = (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), src.Status))
                .AfterMap((src, dest) => dest.Movimentacao = new Movimentacao())
                .AfterMap((src, dest) => dest.Movimentacao.IdItemMovimentacao = src.IdItemMovimentacao)
                .AfterMap((src, dest) => dest.Movimentacao.DataReferencia = src.DataReferencia)
                .AfterMap((src, dest) => dest.Movimentacao.Observacao = src.Observacao)
                .AfterMap((src, dest) => dest.Movimentacao.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade)); ;

            CreateMap<DeleteMovimentacaoPrevistaCommand, MovimentacaoPrevista>()
                .AfterMap((src, dest) => dest.Movimentacao = null);
            #endregion
        }
    }
}