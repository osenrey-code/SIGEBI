using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Handlers;
using SIGEBI.AppEscritorio.Services;
using System.IO;

namespace SIGEBI.AppEscritorio
{
    public static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<JwtBearerHandler>();

            var apiBase = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:54538/";

            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            })
            .AddHttpMessageHandler<JwtBearerHandler>();

            ServiceProvider = services.BuildServiceProvider();
         
        }
    }
}