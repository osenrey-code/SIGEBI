using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.UseCase;
using SIGEBI.Application.UseCase.Auditory;
using SIGEBI.Application.UseCase.Catalogo;
using SIGEBI.Application.UseCase.Devoluciones;
using SIGEBI.Application.UseCase.Penalizaciones;
using SIGEBI.Application.UseCase.Prestamos;
using SIGEBI.Application.UseCase.Reportes;
using SIGEBI.Application.UseCase.Usuarios;

namespace SIGEBI.Application.Dependency
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {

            services.AddUsuariosUseCases();
            services.AddPrestamosUseCase();
            services.AddDevolucionesUseCases();
            services.AddReportesUseCases();
            services.AddCatalogoUseCases();
            services.AddPenalizacionUseCases();
            services.AddAuditoriaUseCases();




            return services;

        }



        //Gestion de Prestamos
        private static IServiceCollection AddPrestamosUseCase(this IServiceCollection services)
        {
            services.AddScoped<SolicitarPrestamo>();
            services.AddScoped<AprobarPrestamo>();
            services.AddScoped<ConsultarHistorialPrestamos>();
            services.AddScoped<ConsultarPrestamosActivos>();

            return services;
        }

        //Gestion de Usuarios 
        private static IServiceCollection AddUsuariosUseCases(this IServiceCollection services)
        {
            services.AddScoped<RegistrarUsuario>();
            services.AddScoped<ActualizarUsuario>();
            services.AddScoped<DesactivarUsuario>();
            services.AddScoped<ActualizarUsuario>();
            services.AddScoped<ConsultarUsuarios>();

            return services;
        }

        //Gestion de Devolucion
        private static IServiceCollection AddDevolucionesUseCases(this IServiceCollection services)
        {
            services.AddScoped<RegistrarDevoluciones>();
            services.AddScoped<ConsultarHistorialDevoluciones>();

            return services;
        }

        //Gestion de Reportes
        private static IServiceCollection AddReportesUseCases(this IServiceCollection services)
        {
            services.AddScoped<GenerarReporteInventario>();
            services.AddScoped<GenerarReportePenalizaciones>();
            services.AddScoped<GenerarReportePrestamo>();
            services.AddScoped<GenerarReportesUsoCatalogo>();

            return services;
        }

        //Gestion de Recursos
        private static IServiceCollection AddCatalogoUseCases(this IServiceCollection services)
        {
            services.AddScoped<ActualizarRecurso>();
            services.AddScoped<CambiarEstadoRecurso>();
            services.AddScoped<ConsultarCatalogo>();
            services.AddScoped<ConsultarDetalleRecurso>();
            services.AddScoped<ConsultarHistorialRecurso>();
            services.AddScoped<EliminarRecurso>();
            services.AddScoped<RegistrarRecurso>();

            return services;
        }

        //Gestion de Penalizaciones
        private static IServiceCollection AddPenalizacionUseCases(this IServiceCollection services)
        {
            services.AddScoped<ConsultarPenalizaciones>();
            services.AddScoped<ConsultarPenalizacionesActivas>();
            services.AddScoped<ResolverPenalizacion>();

            return services;
        }

        //Gestion de Auditoria
        private static IServiceCollection AddAuditoriaUseCases(this IServiceCollection services)

        {
            services.AddScoped<ConsultarLogAuditoria>();

            return services;
        }
    }
}
