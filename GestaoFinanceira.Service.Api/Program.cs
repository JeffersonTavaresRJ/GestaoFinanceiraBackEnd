using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace GestaoFinanceira.Service.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //habilitar o uso de acesso a banco de dados pelo wwwroot local..
            var host = new WebHostBuilder()
                     .UseKestrel()
                     .UseContentRoot(Directory.GetCurrentDirectory())
                     .UseIISIntegration()
                     .UseStartup<Startup>()
                     .Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
