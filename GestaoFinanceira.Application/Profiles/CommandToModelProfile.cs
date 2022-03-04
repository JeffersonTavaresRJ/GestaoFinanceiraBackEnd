using AutoMapper;
using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Application.Commands.FormaPagamento;
using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using GestaoFinanceira.Infra.CrossCutting.Security;
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
                .AfterMap((src, dest) => dest.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((src, dest) => dest.Status = true);
            CreateMap<UpdateCategoriaCommand, Categoria>()
                .AfterMap((src, dest) => dest.IdUsuario = UserEntity.IdUsuario);
            CreateMap<DeleteCategoriaCommand, Categoria>();
            
            #endregion

            #region Conta
            CreateMap<CreateContaCommand, Conta>()
                .AfterMap((src, dest) => dest.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((src, dest)=>dest.Status = true);
            CreateMap<UpdateContaCommand, Conta>()
                .AfterMap((src, dest) => dest.IdUsuario = UserEntity.IdUsuario);
            CreateMap<DeleteContaCommand, Conta>();
            #endregion

            #region FormaPagamento
            CreateMap<CreateFormaPagamentoCommand, FormaPagamento>()
                .AfterMap((src, dest) => dest.IdUsuario = UserEntity.IdUsuario)
                .AfterMap((src, dest)=>dest.Status = true);
            CreateMap<UpdateFormaPagamentoCommand, FormaPagamento>()
                .AfterMap((src, dest) => dest.IdUsuario = UserEntity.IdUsuario);
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
            CreateMap<MovimentacaoPrevistaCommand, Movimentacao>()
                .AfterMap((src, dest) => dest.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade))
                .AfterMap((src, dest) => dest.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0));
                
            CreateMap<UpdateMovimentacaoPrevistaCommand, Movimentacao>()
                .AfterMap((src, dest) => dest.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade))
                .AfterMap((src, dest) => dest.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0));

            #endregion

            #region MovimentacaoPrevista
            CreateMap<MovimentacaoPrevistaCommand, MovimentacaoPrevista>()
                .AfterMap((src, dest) => dest.Movimentacao = new Movimentacao())
                .AfterMap((src, dest) => dest.Movimentacao.IdItemMovimentacao = src.IdItemMovimentacao)
                .AfterMap((src, dest) => dest.Movimentacao.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.Movimentacao.Observacao = src.Observacao)
                .AfterMap((src, dest) => dest.Movimentacao.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade))
                .AfterMap((src, dest) => dest.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.Status = StatusMovimentacaoPrevista.A)
                .AfterMap((src, dest) => dest.DataVencimento = new DateTime(src.DataVencimento.Year, src.DataVencimento.Month, src.DataVencimento.Day,0,0,0));

            CreateMap<UpdateMovimentacaoPrevistaCommand, MovimentacaoPrevista>()
                .AfterMap((src, dest) => dest.Movimentacao = new Movimentacao())
                .AfterMap((src, dest) => dest.Movimentacao.IdItemMovimentacao = src.IdItemMovimentacao)
                .AfterMap((src, dest) => dest.Movimentacao.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.Movimentacao.Observacao = src.Observacao)
                .AfterMap((src, dest) => dest.Movimentacao.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade))
                .AfterMap((src, dest) => dest.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.DataVencimento = new DateTime(src.DataVencimento.Year, src.DataVencimento.Month, src.DataVencimento.Day,0,0,0))
                .AfterMap((src, dest) => dest.Status = (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), src.Status))
                //parcelas não são alteradas na API (Tratamento para não criticar na Validação)
                .AfterMap((src, dest) => dest.NrParcela = 1)
                .AfterMap((src, dest) => dest.NrParcelaTotal = 1);

            CreateMap<DeleteMovimentacaoPrevistaCommand, MovimentacaoPrevista>();
            #endregion

            #region MovimentacaoRealizada       
            CreateMap<MovimentacaoRealizadaCommand, MovimentacaoRealizada>()
                .AfterMap((src, dest) => dest.Movimentacao = new Movimentacao())
                .AfterMap((src, dest) => dest.Movimentacao.IdItemMovimentacao = src.IdItemMovimentacao)
                .AfterMap((src, dest) => dest.Movimentacao.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.Movimentacao.Observacao = src.Observacao)
                .AfterMap((src, dest) => dest.Movimentacao.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade))
                .AfterMap((src, dest) => dest.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.DataMovimentacaoRealizada = new DateTime(src.DataMovimentacaoRealizada.Year, src.DataMovimentacaoRealizada.Month, src.DataMovimentacaoRealizada.Day,0,0,0));

            CreateMap<UpdateMovimentacaoRealizadaCommand, MovimentacaoRealizada>()
                .AfterMap((src, dest) => dest.Movimentacao = new Movimentacao())
                .AfterMap((src, dest) => dest.Movimentacao.IdItemMovimentacao = src.IdItemMovimentacao)
                .AfterMap((src, dest) => dest.Movimentacao.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.Movimentacao.Observacao = src.Observacao)
                .AfterMap((src, dest) => dest.Movimentacao.TipoPrioridade = (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), src.TipoPrioridade))
                .AfterMap((src, dest) => dest.DataReferencia = new DateTime(src.DataReferencia.Year, src.DataReferencia.Month, src.DataReferencia.Day,0,0,0))
                .AfterMap((src, dest) => dest.DataMovimentacaoRealizada = new DateTime(src.DataMovimentacaoRealizada.Year, src.DataMovimentacaoRealizada.Month, src.DataMovimentacaoRealizada.Day,0,0,0));
            #endregion

        }
    }
}