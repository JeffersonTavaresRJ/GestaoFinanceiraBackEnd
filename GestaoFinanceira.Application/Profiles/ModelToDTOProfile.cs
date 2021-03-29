using AutoMapper;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Profiles
{
    public class ModelToDTOProfile : Profile
    {
        public ModelToDTOProfile()
        {
            CreateMap<Categoria, CategoriaDTO>()
                .AfterMap((scr, dest) => dest.Tipo = ExtensionEnum.ObterDescricao((TipoCategoria)Enum.Parse(typeof(TipoCategoria), dest.Tipo.ToString())));

            CreateMap<Usuario, UsuarioDTO>();
                
        }
    }
}
