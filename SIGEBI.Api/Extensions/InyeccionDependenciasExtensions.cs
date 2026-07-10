using SIGEBI.Application.Common;
using SIGEBI.Application.UseCase.Auditory;
using SIGEBI.Application.UseCase.Catalogo;
using SIGEBI.Application.UseCase.Devoluciones;
using SIGEBI.Application.UseCase.Penalizaciones;
using SIGEBI.Application.UseCase.Prestamos;
using SIGEBI.Application.UseCase.Reportes;
using SIGEBI.Application.UseCase.Usuarios;

namespace SIGEBI.Api.Extensions
{
    public static class InyeccionDependenciasExtensions
    {
        public static IServiceCollection AddApplicationUseCases(
            this IServiceCollection services)
        {
            // Validadores comunes
            services.AddScoped<ValidadorReportes>();

            // Usuarios
            services.AddScoped<RegistrarUsuario>();
            services.AddScoped<ActualizarUsuario>();
            services.AddScoped<DesactivarUsuario>();
            services.AddScoped<ConsultarUsuarios>();
            services.AddScoped<AutenticarUsuario>();

            // Catálogo / Recursos
            services.AddScoped<RegistrarRecurso>();
            services.AddScoped<ActualizarRecurso>();
            services.AddScoped<CambiarEstadoRecurso>();
            services.AddScoped<ConsultarCatalogo>();
            services.AddScoped<ConsultarDetalleRecurso>();
            services.AddScoped<ConsultarHistorialRecurso>();
            services.AddScoped<EliminarRecurso>();

            // Categorías
            services.AddScoped<RegistrarCategoria>();
            services.AddScoped<ConsultarCategorias>();

            // Préstamos
            services.AddScoped<SolicitarPrestamo>();
            services.AddScoped<AprobarPrestamo>();
            services.AddScoped<ConsultarPrestamosActivos>();
            services.AddScoped<ConsultarHistorialPrestamos>();

            // Devoluciones
            services.AddScoped<RegistrarDevoluciones>();
            services.AddScoped<ConsultarHistorialDevoluciones>();

            // Penalizaciones
            services.AddScoped<ConsultarPenalizaciones>();
            services.AddScoped<ConsultarPenalizacionesActivas>();
            services.AddScoped<ResolverPenalizacion>();

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
