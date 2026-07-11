using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.UseCase;
using SIGEBI.Application.UseCase.Catalogo;
using SIGEBI.Application.UseCase.Devoluciones;
using SIGEBI.Application.UseCase.Prestamos;
using SIGEBI.Application.UseCase.Reportes;
using SIGEBI.Application.UseCase.Usuarios;

namespace SIGEBI.Application.Dependency
{
    public static class DependencyInjection 
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {

            services.AddScoped<IGestionUsuariosUseCase, GestionUsuariosUseCase>();
            services.AddScoped<IGestionPrestamos, GestionPrestamosUseCase>();
            services.AddScoped<IGestionDevolucionesUseCase, GestionDevolucionesUseCase>();
            services.AddReportesUseCases();





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

    }
}
