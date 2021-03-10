using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Service.Api.Configurations
{
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            //definindo uma política de CORS
            services.AddCors(
                s => s.AddPolicy("DefaultPolicy",
                builder =>
                {
                    builder.AllowAnyOrigin() // qualquer host pode fz requisção para a api
                           .AllowAnyMethod() // qualquer método (POTS, PUT, GET, DELETE..)
                           .AllowAnyHeader(); //enviar informações de cabeçalho..
                }));
        }

        public static void UseCorsSetup(this IApplicationBuilder app)
        {
            app.UseCors("DefaultPolicy");
        }
    }
}