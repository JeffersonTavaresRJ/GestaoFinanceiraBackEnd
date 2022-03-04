using GestaoFinanceira.Infra.CrossCutting.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GestaoFinanceira.Service.Api.Configurations
{
    public static class JwtBearerSetup
    {
        public static void AddJwtBearerSetup(IServiceCollection services, IConfiguration configuration)
        {
            //lendo o código de segurança (Secret) contido no appsettings.json
            var settingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(settingsSection);

            var appSettings = settingsSection.Get<AppSetting>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(
                auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    bearer =>
                    {
                        bearer.RequireHttpsMetadata = false;//requer que a api esteja publicada HTTP ?
                        bearer.SaveToken = true;
                        bearer.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),                            
                            ValidateIssuer = false, // definição de qual usuário vai acessar a api
                            ValidateAudience = false
                        };
                    }
                );

            services.AddTransient(map => new TokenService(appSettings));
        }

        public static void UseJwtBearerSetup(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
