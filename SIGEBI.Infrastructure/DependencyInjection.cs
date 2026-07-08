using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Infrastructure.Repositories;
using SIGEBI.Infrastructure.Services;
using SIGEBI.Application.Services;

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

            services.AddScoped<IUsuario, RepositorioUsuario>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();
            services.AddScoped<IRepositorioRecurso, RepositorioRecurso>();
            services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
            services.AddScoped<IRepositorioPrestamo, RepositorioPrestamo>();
            services.AddScoped<IRepositorioPenalizacion, RepositorioPenalizacion>();
            services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();
            // services.AddScoped<IRepositorioNotificacion, RepositorioNotificacion>();

            services.AddScoped<IServicioCorreo, ServicioCorreoSMTP>();
            services.AddScoped<IServicioToken, ServicioToken>();
            services.AddScoped<IServicioPassword, ServicioPassword>();
            services.AddScoped<IStorageService, LocalStorageService>();

            return services;
        }
    }
}
