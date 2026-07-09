using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Common;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Services;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Infrastructure.Repositories;
using SIGEBI.Infrastructure.Services;
using QuestPDF.Infrastructure;

namespace SIGEBI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<SIGEBIDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ConexionSql")
                )
            );

            //Repositorios 
            services.AddScoped<IUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();
            services.AddScoped<IRepositorioRecurso, RepositorioRecurso>();
            services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
            services.AddScoped<IRepositorioPrestamo, RepositorioPrestamo>();
            services.AddScoped<IRepositorioPenalizacion, RepositorioPenalizacion>();
            services.AddScoped<IRepositorioNotificacion, RepositorioNotificacion>();
            services.AddScoped<IEjemplarRepository, RepositorioEjemplar>();
            services.AddScoped<ISolicitudRepository, RepositorioSolicitud>();
            services.AddScoped<IRepositorioDevolucion, RepositorioDevolucion>();
            services.AddScoped<IRepositorioReporte, RepositorioReporte>();

            //Servicios de aplicacion
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();

            //Servicios Tecnicos
            services.AddScoped<IServicioCorreo, ServicioCorreoSMTP>();
            services.AddScoped<IServicioToken, ServicioToken>();
            services.AddScoped<IServicioPassword, ServicioPassword>();
            services.AddScoped<IStorageService, LocalStorageService>();

            QuestPDF.Settings.License = LicenseType.Community;
            services.AddScoped<IExportadorReportePdf, ExportadorReportePdf>();

            return services;
        }
    }
}
