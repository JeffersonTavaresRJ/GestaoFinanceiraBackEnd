using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GestaoFinanceira.Infra.IoC;
using GestaoFinanceira.Service.Api.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoFinanceira.Services.Api.Configurations;

namespace GestaoFinanceira.Service.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //setup para configuração Swagger..
            SwaggerSetup.AddSwaggerSetup(services);
            //setup para configuração do EntityFramework..
            EntityFrameworkSetup.AddEntityFrameworkSetup(services, Configuration);
            //setup para configuração de injeção de dependencia..
            InjecaoDependencia.Registrar(services);
            //setup para JWT Bearer..
            JwtBearerSetup.AddJwtBearerSetup(services, Configuration);
            //setup para MongoDB..
            MongoDBSetup.AddMongoDBSetup(services, Configuration);
            //setup para MediatR..
            MediatRSetup.AddMediatRSetup(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();--antiga autorização para swagger(deve ser retirada para o JWT funcionar)

            //setup para configuração do JWT bearer..
            JwtBearerSetup.UseJwtBearerSetup(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });           

            //setup para configuração do Swagger..
            SwaggerSetup.UseSwaggerSetup(app);

            
        }
    }
}
