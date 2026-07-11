using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.UseCase;
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

            services.AddScoped<IGestionUsuariosUseCase, GestionUsuarios>();
            services.AddScoped<IGestionPrestamos, GestionPrestamos>();
            services.AddScoped<IGestionDevolucionesUseCase, GestionDevoluciones>();
            services.AddScoped<IGestionReportesUseCase, GestionReportes>();





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
    }
}
