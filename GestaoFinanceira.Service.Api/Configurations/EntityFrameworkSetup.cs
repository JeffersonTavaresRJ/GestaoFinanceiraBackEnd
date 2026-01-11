using GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace GestaoFinanceira.Service.Api.Configurations
{
    public static class EntityFrameworkSetup
    {
        public static void AddEntityFrameworkSetup
            (this IServiceCollection services, IConfiguration configuration)
        {
            /*
             * Estamos configurando a classe SqlContext 
             * criada no projeto Infra.Data
             * passando para esta classe o caminho da connectionstring do BD
             */
            //TODO: LOG DE EXECUÇÃO DO EF CORE - PASSO 02: CONFIGURAR O ADDLOGGING NO service DO STARTUP..
            services.AddLogging(loggingBuilder => {
                loggingBuilder.AddConsole()
                              .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                              .AddDebug();
            });

            //services.AddControllers().AddNewtonsoftJson(x =>x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<SqlContext>(
                options => { options.UseSqlServer(configuration.GetConnectionString("GestaoFinanceira"),
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                // Ativa a resiliência para falhas temporárias (comum no Docker)
                                sqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 10,           // Tenta até 10 vezes
                                    maxRetryDelay: TimeSpan.FromSeconds(30), // Espera até 30s entre as tentativas
                                    errorNumbersToAdd: null     // Adicione códigos de erro SQL específicos se necessário
                                );
                            });
                    //TODO: LOG DE EXECUÇÃO DO EF CORE - PASSO 03: ADICIONAR O EnableSensitiveDataLogging(true) no OPTIONS..
                    options.EnableSensitiveDataLogging(false);
                });

        }
    }
}
