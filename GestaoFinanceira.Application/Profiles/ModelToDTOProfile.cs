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

        }
    }
}
