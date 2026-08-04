using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioEjemplar : RepositorioBase<Ejemplar>, IEjemplarRepository
    {
        private readonly ILogger<RepositorioEjemplar> _loggerEjemplar;

        public RepositorioEjemplar(SIGEBIDbContext context, ILogger<RepositorioEjemplar> logger) : base(context, logger)
        {
            _loggerEjemplar = logger;
        }

        public async Task<Ejemplar?> ObtenerPorIdAsync(int id)
        {
            try
            {
                _loggerEjemplar.LogInformation("Obteniendo ejemplar con recursos bibliográficos para ID: {Id}", id);
                return await _context.Ejemplares
                    .Include(e => e.RecursoBibliografico)
                    .FirstOrDefaultAsync(e => e.EjemplarId == id);
            }
            catch (Exception ex)
            {
                _loggerEjemplar.LogError(ex, "Error al obtener ejemplar con ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}