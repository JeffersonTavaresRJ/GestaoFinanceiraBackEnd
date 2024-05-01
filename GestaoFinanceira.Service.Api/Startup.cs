using GestaoFinanceira.Infra.IoC;
using GestaoFinanceira.Service.Api.Configurations;
using GestaoFinanceira.Services.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System.Globalization;

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
            services.AddControllersWithViews(options =>
                 options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>());
            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllers();
            //setup para configuração Swagger..
            SwaggerSetup.AddSwaggerSetup(services);
            //setup para configuração do EntityFramework..
            EntityFrameworkSetup.AddEntityFrameworkSetup(services, Configuration);
            //setup para configuração de injeção de dependencia..
            InjecaoDependencia.Registrar(services, Configuration);
            //setup para JWT Bearer..
            JwtBearerSetup.AddJwtBearerSetup(services, Configuration);
            //setup para MongoDB..
            MongoDBSetup.AddMongoDBSetup(services, Configuration);
            //setup para MediatR..
            MediatRSetup.AddMediatRSetup(services);
            //setup para AutoMapper..
            AutoMapperSetup.AddAutoMapperSetup(services);
            //Setup para o Cors
            CorsSetup.AddCorsSetup(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
                IdentityModelEventSource.ShowPII = true; //capturar erros PII (expiração token)
            }

            

            CorsSetup.UseCorsSetup(app);//deve ficar aqui esta linha

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

            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}
