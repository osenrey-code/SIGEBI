using SIGEBI.Application.UseCase.Devoluciones;
using SIGEBI.Application.UseCase.Prestamos;
using SIGEBI.Application.UseCase.Usuarios;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Services;
namespace SIGEBI.Api.Extensions
{
    public static class InyeccionDependenciasExtensions
    {
        public static IServiceCollection AddApplicationUseCases(
            this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();
            // Usuarios
            services.AddScoped<RegistrarUsuario>();
            services.AddScoped<RegistrarUsuarioWeb>();
            services.AddScoped<ActualizarUsuario>();
            services.AddScoped<DesactivarUsuario>();
            services.AddScoped<ConsultarUsuarios>();
            services.AddScoped<AsignarPerfilLector>();
            services.AddScoped<AutenticarUsuario>();

            // Préstamos
            services.AddScoped<SolicitarPrestamo>();
            services.AddScoped<AprobarPrestamo>();
            services.AddScoped<RechazarPrestamo>();
            services.AddScoped<ConsultarPrestamoPorId>();
            services.AddScoped<ConsultarPrestamosActivos>();
            services.AddScoped<ConsultarHistorialPrestamos>();
            services.AddScoped<RegistrarPrestamoPresencial>();

            // Devoluciones
            services.AddScoped<RegistrarDevoluciones>();

            return services;
        }
    }
}
