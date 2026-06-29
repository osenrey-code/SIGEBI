using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Infrastructure.Repositories;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Infrastructure.Services;

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
            services.AddScoped<IRepositorioPerfilLector, RepositorioPerfilLector>();

            services.AddScoped<IRepositorioRecurso, RepositorioRecurso>();
            services.AddScoped<IRepositorioPrestamo, RepositorioPrestamo>();
            services.AddScoped<IRepositorioPenalizacion, RepositorioPenalizacion>();

            services.AddScoped<IRepositorioNotificacion, RepositorioNotificacion>();
            services.AddScoped<IServicioCorreo, ServicioCorreoSMTP>();
            return services;
        }
    }
}
