using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Common;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.Service;
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
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddScoped<IGestionUsuariosUseCase, GestionUsuarios>();
            services.AddScoped<IGestionPrestamos, GestionPrestamos>();
            services.AddScoped<IGestionDevolucionesUseCase, GestionDevoluciones>();
            services.AddScoped<IGestionReportesUseCase, GestionReportes>();
            services.AddScoped<IGestionCatalogo,GestionCatalogo>();
            services.AddScoped<IGestionPenalizaciones, GestionPenalizaciones>();
            services.AddScoped<IGestionCategorias, GestionCategorias>();
            services.AddScoped<ILogin, Login>();
            services.AddScoped<ILogAuditoria, ConsultarLogAuditoria>();
            services.AddScoped<IServicioNotificacion, ServicioNotificacion>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<ValidadorReportes>();

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
