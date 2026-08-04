using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioDevolucion : RepositorioBase<Devolucion>, IRepositorioDevolucion
    {
        private readonly ILogger<RepositorioDevolucion> _loggerDevolucion;

        public RepositorioDevolucion(SIGEBIDbContext context, ILogger<RepositorioDevolucion> logger) : base(context, logger)
        {
            _loggerDevolucion = logger;
        }

        public async Task<IEnumerable<Devolucion>> ConsultarHistorialAsync(
            int? usuarioId,
            int? recursoBibliograficoId,
            int? ejemplarId,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            try
            {
                _loggerDevolucion.LogInformation("Consultando historial de devoluciones con filtros.");

                var query = _dbSet
                    .AsNoTracking()
                    .Include(d => d.Prestamo)
                        .ThenInclude(p => p!.Usuario)
                    .Include(d => d.Prestamo)
                        .ThenInclude(p => p!.Ejemplar)
                            .ThenInclude(e => e!.RecursoBibliografico)
                    .AsQueryable();

                if (usuarioId.HasValue)
                {
                    query = query.Where(d =>
                        d.Prestamo != null &&
                        d.Prestamo.UsuarioId == usuarioId.Value);
                }

                if (recursoBibliograficoId.HasValue)
                {
                    query = query.Where(d =>
                        d.Prestamo != null &&
                        d.Prestamo.Ejemplar != null &&
                        d.Prestamo.Ejemplar.RecursoBibliograficoId == recursoBibliograficoId.Value);
                }

                if (ejemplarId.HasValue)
                {
                    query = query.Where(d =>
                        d.Prestamo != null &&
                        d.Prestamo.EjemplarId == ejemplarId.Value);
                }

                if (fechaInicio.HasValue)
                {
                    query = query.Where(d =>
                        d.FechaDevolucion >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    query = query.Where(d =>
                        d.FechaDevolucion <= fechaFin.Value);
                }

                var resultado = await query
                    .OrderByDescending(d => d.FechaDevolucion)
                    .ToListAsync();

                _loggerDevolucion.LogInformation("Historial de devoluciones consultado exitosamente. Total registros: {Count}", resultado.Count);
                return resultado;
            }
            catch (Exception ex)
            {
                _loggerDevolucion.LogError(ex, "Error al consultar el historial de devoluciones: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Devolucion?> ObtenerPorIdAsync(int devolucionId)
        {
            try
            {
                _loggerDevolucion.LogInformation("Obteniendo devolución con ID: {DevolucionId}", devolucionId);

                return await _context.Devoluciones
                   .AsNoTracking()
                   .Include(d => d.Prestamo)
                       .ThenInclude(p => p!.Usuario)
                   .Include(d => d.Prestamo)
                       .ThenInclude(p => p!.Ejemplar)
                           .ThenInclude(e => e!.RecursoBibliografico)
                   .FirstOrDefaultAsync(d => d.DevolucionId == devolucionId);
            }
            catch (Exception ex)
            {
                _loggerDevolucion.LogError(ex, "Error al obtener la devolución con ID {DevolucionId}: {Message}", devolucionId, ex.Message);
                throw;
            }
        }

        public async Task<Devolucion?> ObtenerPorPrestamoIdAsync(int prestamoId)
        {
            try
            {
                _loggerDevolucion.LogInformation("Obteniendo devolución por PrestamoId: {PrestamoId}", prestamoId);

                return await _context.Devoluciones
                    .AsNoTracking()
                    .Include(d => d.Prestamo)
                        .ThenInclude(p => p!.Usuario)
                    .Include(d => d.Prestamo)
                        .ThenInclude(p => p!.Ejemplar)
                            .ThenInclude(e => e!.RecursoBibliografico)
                    .FirstOrDefaultAsync(d => d.PrestamoId == prestamoId);
            }
            catch (Exception ex)
            {
                _loggerDevolucion.LogError(ex, "Error al obtener la devolución por PrestamoId {PrestamoId}: {Message}", prestamoId, ex.Message);
                throw;
            }
        }
    }
}