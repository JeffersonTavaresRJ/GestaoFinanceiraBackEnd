﻿using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Application.Services;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Interfaces.Cryptography;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Services;
using GestaoFinanceira.Infra.Caching.Repositories;
using GestaoFinanceira.Infra.CrossCutting.Cryptography;
using GestaoFinanceira.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoFinanceira.Infra.IoC
{
    public static class InjecaoDependencia
    {
        public static void Registrar(IServiceCollection services)
        {
            //TODO: Injecao de Dependencia - tratamento para instanciar automaticamente os objetos

            /*
             * Transient
            Um novo objeto é criado toda vez que for requisitado. 
            Utiliza o Dispose para destruir o conteúdo de um objeto criado
            */

            #region Application
            services.AddTransient<IUsuarioApplicationService, UsuarioApplicationService>();
            services.AddTransient<ICategoriaApplicationService, CategoriaApplicationService>();
            #endregion

            #region Domain
            services.AddTransient<ICategoriaDomainService, CategoriaDomainService>();
            services.AddTransient<IUsuarioDomainService, UsuarioDomainService>();
            #endregion

            #region InfraData
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            #region InfraCaching
            services.AddTransient<ICategoriaCaching, CategoriaCaching>();
            #endregion

            #region InfraCryptography
            services.AddTransient<IMD5Service, MD5Service>();
            #endregion
        }
    }
}
