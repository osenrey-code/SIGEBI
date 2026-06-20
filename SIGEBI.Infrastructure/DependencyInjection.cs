using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Infrastructure.Persistencia;
using SIGEBI.Infrastructure.Repositories;
using System.Collections.Generic;
using SIGEBI.Domain.Entities;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<SIGEBIDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioPrestamo, RepositorioPrestamo>();
            services.AddScoped<IRepositorioPerfilLector, RepositorioPerfilLector>();


            return services;
        }
    }
}
