using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Extensions;
using SIGEBI.AppEscritorio.Views.Shared;
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
            ConfigurarManejoDeErrores();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddAppServices(configuration);

            ServiceProvider = services.BuildServiceProvider();
            var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
            Application.Run(loginForm);

        }

        private static void ConfigurarManejoDeErrores()
        {
            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show($"Ocurrió un error insperado en la aplicación:\n\n{e.Exception.Message}",
                                  "Error del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error crítico no controlado:\n\n{ex.Message}",
                                     "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            };
        }
    }
}