using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.AppEscritorio.Handlers;
using SIGEBI.AppEscritorio.Services;
using SIGEBI.AppEscritorio.Services.Api;
using SIGEBI.AppEscritorio.Services.Auditoria;
using SIGEBI.AppEscritorio.Services.Auth;
using SIGEBI.AppEscritorio.Services.Catalogo;
using SIGEBI.AppEscritorio.Services.Categoria;
using SIGEBI.AppEscritorio.Services.Categorias;
using SIGEBI.AppEscritorio.Services.Devolucion;
using SIGEBI.AppEscritorio.Services.Interfaces;
using SIGEBI.AppEscritorio.Services.Penalizaciones;
using SIGEBI.AppEscritorio.Services.Prestamo;
using SIGEBI.AppEscritorio.Services.Reporte;
using SIGEBI.AppEscritorio.Services.Usuario;
using SIGEBI.AppEscritorio.Views.Categorias;
using SIGEBI.AppEscritorio.Views.Devolucion;
using SIGEBI.AppEscritorio.Views.Penalizaciones;
using SIGEBI.AppEscritorio.Views.Prestamo;
using SIGEBI.AppEscritorio.Views.Reportes;
using SIGEBI.AppEscritorio.Views.Shared;
using SIGEBI.AppEscritorio.Views.Usuario;
using SIGEBI.AppEscritorio.Views.Auditoria;
using SIGEBI.AppEscritorio.Views.Penalizacion;

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
            services.AddTransient<IPrestamoService, PrestamoService>();
            services.AddTransient<IReporteService, ReporteService>();
            services.AddTransient<IDevolucionService, DevolucionService>();
            services.AddTransient<IPenalizacionService, PenalizacionService>();
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IAuditoriaService, AuditoriaService>();

            return services;
        }

        public static IServiceCollection AddFormViews(this IServiceCollection services)
        {
            services.AddTransient<LoginForm>();
            services.AddTransient<Main>();
            services.AddTransient<CatalogoForm>();
            services.AddTransient<GestionarRecursoForm>();
            services.AddTransient<PrestamoForm>();
            services.AddTransient<UsuarioForm>();
            services.AddTransient<ReporteForm>();
            services.AddTransient<DevolucionForm>();
            services.AddTransient<PenalizacionForm>();
            services.AddTransient<CategoriaForm>();
            services.AddTransient<AuditoriaForm>();
            services.AddTransient<DetalleSolicitud>();
            services.AddTransient<DetallePrestamo>();
            services.AddTransient<DetallePenalizacion>();
            return services;

        }
    }
}