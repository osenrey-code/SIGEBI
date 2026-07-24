using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Common;
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
            //Usuarios
            services.AddScoped<ILogin, Login>();
            services.AddScoped<IGestionUsuariosUseCase, GestionUsuarios>();

            //Prestamos
            services.AddScoped<IGestionPrestamos, GestionPrestamos>();
            services.AddScoped<IGestionDevolucionesUseCase, GestionDevoluciones>();

            //Reportes
            services.AddScoped<IGestionReportesUseCase, GestionReportes>();
            services.AddScoped<ValidadorReportes>();

            //Catalogo
            services.AddScoped<IGestionCatalogo,GestionCatalogo>();
            services.AddScoped<IGestionPenalizaciones, GestionPenalizaciones>();
            services.AddScoped<IGestionCategorias, GestionCategorias>();

            //Auditoria y servicios 
            services.AddScoped<ILogAuditoria, ConsultarLogAuditoria>();
            services.AddScoped<IServicioNotificacion, ServicioNotificacion>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            
            return services;
        }
    }
}
