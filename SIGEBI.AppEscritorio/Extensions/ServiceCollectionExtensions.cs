using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Handlers;
using SIGEBI.AppEscritorio.Services.Api;
using SIGEBI.AppEscritorio.Services.Auth;
using SIGEBI.AppEscritorio.Services.Implementaciones;
using SIGEBI.AppEscritorio.Services.Interfaces; 
using SIGEBI.AppEscritorio.Views;
using SIGEBI.AppEscritorio.Views.Administrador;
using SIGEBI.AppEscritorio.Views.Auditor;
using SIGEBI.AppEscritorio.Views.Bibliotecario;
using SIGEBI.AppEscritorio.Views.Shared;
using SIGEBI.AppEscritorio.Views.Usuario;
using SIGEBI.AppEscritorio.Services.Usuario;

namespace SIGEBI.AppEscritorio.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiServices(configuration);
            services.AddApplicationServices();
            services.AddFormViews();

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<JwtBearerHandler>();

            var apiBase = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:54538/";

            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            })
            .AddHttpMessageHandler<JwtBearerHandler>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<ICatalogoService, CatalogoService>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            return services;
        }

        public static IServiceCollection AddFormViews(this IServiceCollection services)
        {
            services.AddTransient<LoginForm>();
            services.AddTransient<CatalogoForm>();
            services.AddTransient<GestionarRecursoForm>();

            services.AddTransient<UsuarioForm>();

            return services;

        }
    }
}