using AutoMapper;
using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Profiles
{
    public class CommandToModelProfile : Profile
    {
        public CommandToModelProfile()
        {
            #region Usuario 
            CreateMap<CreateUsuarioCommand, Usuario>();

            CreateMap<UpdateUsuarioCommand, Usuario>()
                .AfterMap((src, dest) => dest.Id = int.Parse(src.Id));

            CreateMap<DeleteUsuarioCommand, Usuario>()
                .AfterMap((src, dest) => dest.Id = int.Parse(src.Id));            
            #endregion

            #region Categoria
            CreateMap<CreateCategoriaCommand, Categoria>()
                .AfterMap((src, dest) => dest.Status = true)
                .AfterMap((src, dest) => dest.Tipo = (TipoCategoria)Enum.Parse(typeof(TipoCategoria), src.Tipo))
                .AfterMap((src, dest) => dest.IdUsuario = int.Parse(src.IdUsuario));

            CreateMap<UpdateCategoriaCommand, Categoria>()
                .AfterMap((src, dest) => dest.Id = int.Parse(src.Id))
                .AfterMap((src, dest) => dest.Status = bool.Parse(src.Status))
                .AfterMap((src, dest) => dest.Tipo = (TipoCategoria)Enum.Parse(typeof(TipoCategoria), src.Tipo))
                .AfterMap((src, dest) => dest.IdUsuario = int.Parse(src.IdUsuario));

            CreateMap<DeleteCategoriaCommand, Categoria>()
                .AfterMap((src, dest) => dest.Id = int.Parse(src.Id));
            #endregion
        }
    }
}