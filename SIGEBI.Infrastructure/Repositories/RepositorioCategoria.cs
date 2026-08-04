using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioCategoria : RepositorioBase<Categoria>, IRepositorioCategoria
    {
        private readonly ILogger<RepositorioCategoria> _loggerCategoria;

        public RepositorioCategoria(SIGEBIDbContext context, ILogger<RepositorioCategoria> logger) : base(context, logger)
        {
            _loggerCategoria = logger;
        }

        public async Task<Categoria?> ObtenerPorNombreAsync(string nombre)
        {
            try
            {
                _loggerCategoria.LogInformation("Obteniendo categoría por nombre: {Nombre}", nombre);
                return await _dbSet
                    .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.Trim().ToLower());
            }
            catch (Exception ex)
            {
                _loggerCategoria.LogError(ex, "Error al obtener categoría por nombre {Nombre}: {Message}", nombre, ex.Message);
                throw;
            }
        }
    }
}