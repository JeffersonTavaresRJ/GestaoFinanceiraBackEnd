using AutoMapper.Configuration;
using GestaoFinanceira.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using Dapper;

namespace GestaoFinanceira.Service.Api.Configurations
{
    public static class DapperSetup
    {
        public static void AddDapperSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDapperSetup(configuration);
        }
    }
}
