using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Services;

using SIGEBI.Application.UseCase.Usuarios;
using SIGEBI.Application.UseCase.Prestamos;
using SIGEBI.Application.UseCase.Devoluciones;
using SIGEBI.Application.UseCase.Catalogo;
using SIGEBI.Application.UseCase.Penalizaciones;
using SIGEBI.Application.UseCase.Notificaciones;
using SIGEBI.Application.UseCase.Auditoria;
using SIGEBI.Application.UseCase.Reportes;

namespace SIGEBI.Api.Extensions
{
    public static class InyeccionDependenciasExtensions
    {
        public static IServiceCollection AddApplicationUseCases(
            this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();

            // Usuarios
            services.AddScoped<RegistrarUsuario>();
            services.AddScoped<RegistrarUsuarioWeb>();
            services.AddScoped<ActualizarUsuario>();
            services.AddScoped<DesactivarUsuario>();
            services.AddScoped<ConsultarUsuarios>();
            services.AddScoped<AsignarPerfilLector>();
            services.AddScoped<AutenticarUsuario>();

            // Catálogo / Recursos
            services.AddScoped<RegistrarRecurso>();
            services.AddScoped<ActualizarRecurso>();
            services.AddScoped<CambiarEstadoRecurso>();
            services.AddScoped<ConsultarCatalogo>();
            services.AddScoped<ConsultarDetalleRecurso>();
            services.AddScoped<ConsultarHistorialRecurso>();

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
            services.AddScoped<ConsultarHistorialDevoluciones>();

            // Penalizaciones
            services.AddScoped<ConsultarPenalizaciones>();
            services.AddScoped<ConsultarPenalizacionesActivas>();
            services.AddScoped<ResolverPenalizacion>();

            // Notificaciones
            services.AddScoped<ConsultarNotificaciones>();
            services.AddScoped<EnviarRecordatorioVencimiento>();


            // Auditoría
            services.AddScoped<ConsultarLogAuditoria>();

            // Reportes
            services.AddScoped<GenerarReportePrestamo>();
            services.AddScoped<GenerarReportePenalizaciones>();
            services.AddScoped<GenerarReporteInventario>();
            services.AddScoped<GenerarReportesUsoCatalogo>();

            return services;
        }
    }
}
