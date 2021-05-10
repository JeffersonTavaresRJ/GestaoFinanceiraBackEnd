using AutoMapper;
using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Commands.Conta;
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

            CreateMap<UpdateUsuarioCommand, Usuario>();

            CreateMap<DeleteUsuarioCommand, Usuario>();
            #endregion

            #region Categoria
            CreateMap<CreateCategoriaCommand, Categoria>()
                .AfterMap((src, dest) => dest.Status = true);
            /* .AfterMap((src, dest) => dest.Tipo = (TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), src.Tipo))*/


            CreateMap<UpdateCategoriaCommand, Categoria>();
            /*    .AfterMap((src, dest) => dest.Id = int.Parse(src.Id))*/


            /*  .AfterMap((src, dest) => dest.Tipo = (TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), src.Tipo))*/

            /*  .AfterMap((src, dest) => dest.IdUsuario = int.Parse(src.IdUsuario));*/

            CreateMap<DeleteCategoriaCommand, Categoria>();
            
            #endregion

            #region Conta
            CreateMap<CreateContaCommand, Conta>()
                .AfterMap((src, dest)=>dest.Status = true);
            CreateMap<UpdateContaCommand, Conta>();
            CreateMap<DeleteContaCommand, Conta>();
            #endregion

        }
    }
}