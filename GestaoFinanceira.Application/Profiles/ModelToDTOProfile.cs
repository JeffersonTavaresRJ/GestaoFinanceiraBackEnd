using AutoMapper;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Application.Profiles
{
    public class ModelToDTOProfile : Profile
    {
        public ModelToDTOProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();

            CreateMap<Categoria, CategoriaDTO>();
            /*    .AfterMap((scr, dest) => dest.Tipo = ExtensionEnum.ObterDescricao((TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), dest.Tipo.ToString())));*/

            CreateMap<Conta, ContaDTO>();

            CreateMap<FormaPagamento, FormaPagamentoDTO>();

        }
    }
}
