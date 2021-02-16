using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.Caching.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Services.Api.Configurations
{
    public static class MongoDBSetup
    {
        public static void AddMongoDBSetup(this IServiceCollection services, IConfiguration configuration )
        {
            //Ler os parametros mapeados no arquivo 'appsettings.json'
            //e carregar os seus valores na classe MongoDBSettings
            var dbSettings = new MongoDBSettings();
            new ConfigureFromConfigurationOptions<MongoDBSettings>(configuration.GetSection("MongoDBSettings")).Configure(dbSettings);
            services.AddSingleton(dbSettings);

            //criando uma injeção de dependência Singleton
            //para a classe MongoDBContext
            services.AddSingleton<MongoDBContext>();


        }
    }
}
