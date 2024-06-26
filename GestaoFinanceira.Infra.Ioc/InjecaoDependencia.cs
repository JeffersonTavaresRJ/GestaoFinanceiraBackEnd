﻿using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Application.Services;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Interfaces.Cryptography;
using GestaoFinanceira.Domain.Interfaces.Repositories;
using GestaoFinanceira.Domain.Interfaces.Repositories.Dapper;
using GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Services;
using GestaoFinanceira.Infra.Caching.Repositories;
using GestaoFinanceira.Infra.CrossCutting.Cryptography;
using GestaoFinanceira.Infra.Data.Repositories.Dapper;
using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace GestaoFinanceira.Infra.IoC
{
    public static class InjecaoDependencia
    {
        public static void Registrar(IServiceCollection services, IConfiguration configuration)
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
            services.AddTransient<IContaApplicationService, ContaApplicationService>();
            services.AddTransient<IFormaPagamentoApplicationService, FormaPagamentoApplicationService>();
            services.AddTransient<IItemMovimentacaoApplicationService, ItemMovimentacaoApplicationService>();
            services.AddTransient<IMovimentacaoPrevistaApplicationService, MovimentacaoPrevistaApplicationService>();
            services.AddTransient<IMovimentacaoRealizadaApplicationService, MovimentacaoRealizadaApplicationService>();
            services.AddTransient<IFechamentoApplicationService, FechamentoApplicationService>();
            #endregion

            #region Domain
            services.AddTransient<IContaDomainService, ContaDomainService>();
            services.AddTransient<ICategoriaDomainService, CategoriaDomainService>();
            services.AddTransient<IUsuarioDomainService, UsuarioDomainService>();
            services.AddTransient<IFormaPagamentoDomainService, FormaPagamentoDomainService>();
            services.AddTransient<IItemMovimentacaoDomainService, ItemMovimentacaoDomainService>();
            services.AddTransient<IMovimentacaoDomainService, MovimentacaoDomainService>();
            services.AddTransient<IMovimentacaoPrevistaDomainService, MovimentacaoPrevistaDomainService>();
            services.AddTransient<IMovimentacaoRealizadaDomainService, MovimentacaoRealizadaDomainService>();
            services.AddTransient<ISaldoDiarioDomainService, SaldoDiarioDomainService>();
            services.AddTransient<IFechamentoDomainService, FechamentoDomainService>();
            services.AddTransient<ISaldoContaDomainService, SaldoContaDomainService>();
            services.AddTransient<IItemMovimentacaoMensalDomainService, ItemMovimentacaoMensalDomainService>();

            #endregion

            #region InfraDataEF
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<IContaRepository, ContaRepository>();
            services.AddTransient<IFormaPagamentoRepository, FormaPagamentoRepository>();
            services.AddTransient<IItemMovimentacaoRepository, ItemMovimentacaoRepository>();
            services.AddTransient<IMovimentacaoRepository, MovimentacaoRepository>();
            services.AddTransient<IMovimentacaoPrevistaRepository, MovimentacaoPrevistaRepository>();
            services.AddTransient<IMovimentacaoRealizadaRepository, MovimentacaoRealizadaRepository>();
            services.AddTransient<ISaldoDiarioRepository, SaldoDiarioRepository>();
            services.AddTransient<ISaldoContaRepository, SaldoContaRepository>();
            services.AddTransient<IItemMovimentacaoMensalRepository, ItemMovimentacaoMensalRepository>();            
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            #region InfraDataDapper
            services.AddTransient<IDbConnection>(c => new SqlConnection(configuration.GetConnectionString("GestaoFinanceira")));
            services.AddTransient<ITransferenciaContasRepository, TransferenciaContasRepository>();
            services.AddTransient<IFechamentoRepository, FechamentoRepository>();
            #endregion

            #region InfraCaching
            services.AddTransient<ICategoriaCaching, CategoriaCaching>();
            services.AddTransient<IContaCaching, ContaCaching>();
            services.AddTransient<IFormaPagamentoCaching, FormaPagamentoCaching>();
            services.AddTransient<IItemMovimentacaoCaching, ItemMovimentacaoCaching>();
            services.AddTransient<IMovimentacaoPrevistaCaching, MovimentacaoPrevistaCaching>();
            services.AddTransient<IMovimentacaoRealizadaCaching, MovimentacaoRealizadaCaching>();
            services.AddTransient<IMovimentacaoRealizadaMensalCaching, MovimentacaoRealizadaMensalCaching>();
            services.AddTransient<ISaldoDiarioCaching, SaldoDiarioCaching>();
            #endregion

            #region InfraCryptography
            services.AddTransient<IMD5Service, MD5Service>();
            #endregion
        }
    }
}
