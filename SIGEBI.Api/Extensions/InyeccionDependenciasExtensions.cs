using SIGEBI.Application.UseCase.Devoluciones;
using SIGEBI.Application.UseCase.Prestamos;
using SIGEBI.Application.UseCase.Usuarios;

namespace SIGEBI.Api.Extensions
{
    public static class InyeccionDependenciasExtensions
    {
        public static IServiceCollection AddApplicationUseCases(
            this IServiceCollection services)
        {
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
