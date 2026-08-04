using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPrestamo : RepositorioBase<Prestamo>, IRepositorioPrestamo
    {
        private readonly ILogger<RepositorioPrestamo> _loggerPrestamo;

        public RepositorioPrestamo(SIGEBIDbContext context, ILogger<RepositorioPrestamo> logger) : base(context, logger)
        {
            _loggerPrestamo = logger;
        }

        public async Task<IEnumerable<Prestamo>> ConsultarActivosAsync(int? usuarioId, int? recursoBibliograficoId, int? ejemplarId)
        {
            try
            {
                _loggerPrestamo.LogInformation("Consultando préstamos activos.");

                var query = _context.Prestamos
                    .AsNoTracking()
                    .Include(p => p.Usuario)
                    .Include(p => p.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                    .Where(p => p.Estado == EstadoPrestamo.Activo)
                    .AsQueryable();

                if (usuarioId.HasValue)
                {
                    query = query.Where(p => p.UsuarioId == usuarioId.Value);
                }

                if (recursoBibliograficoId.HasValue)
                {
                    query = query.Where(p =>
                        p.Ejemplar != null &&
                        p.Ejemplar.RecursoBibliograficoId == recursoBibliograficoId.Value);
                }

                if (ejemplarId.HasValue)
                {
                    query = query.Where(p => p.EjemplarId == ejemplarId.Value);
                }

                return await query
                    .OrderByDescending(p => p.FechaInicio)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al consultar préstamos activos: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Prestamo>> ConsultarHistorialAsync(int? usuarioId, int? recursoBibliograficoId, int? ejemplarId)
        {
            try
            {
                _loggerPrestamo.LogInformation("Consultando historial de préstamos.");

                var query = _context.Prestamos
                    .AsNoTracking()
                    .Include(p => p.Usuario)
                    .Include(p => p.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                    .AsQueryable();

                if (usuarioId.HasValue)
                {
                    query = query.Where(p => p.UsuarioId == usuarioId.Value);
                }

                if (recursoBibliograficoId.HasValue)
                {
                    query = query.Where(p =>
                        p.Ejemplar != null &&
                        p.Ejemplar.RecursoBibliograficoId == recursoBibliograficoId.Value);
                }

                if (ejemplarId.HasValue)
                {
                    query = query.Where(p => p.EjemplarId == ejemplarId.Value);
                }

                return await query
                    .OrderByDescending(p => p.FechaInicio)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al consultar historial de préstamos: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<int> ContarActivosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                _loggerPrestamo.LogInformation("Contando préstamos activos para el usuario ID: {UsuarioId}", usuarioId);
                return await _context.Prestamos
                    .CountAsync(p => p.UsuarioId == usuarioId && p.Estado == EstadoPrestamo.Activo);
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al contar préstamos activos por usuario: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                _loggerPrestamo.LogInformation("Obteniendo préstamos activos para el usuario ID: {UsuarioId}", usuarioId);
                return await _context.Prestamos
                    .AsNoTracking()
                    .Where(p => p.UsuarioId == usuarioId && p.Estado == EstadoPrestamo.Activo)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al obtener préstamos activos por usuario: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> TienePrestamoActivoDeRecursoAsync(int usuarioId, int recursoBibliograficoId)
        {
            try
            {
                _loggerPrestamo.LogInformation("Verificando si el usuario {UsuarioId} tiene préstamo activo del recurso {RecursoId}", usuarioId, recursoBibliograficoId);
                return await _dbSet.AnyAsync(p =>
                    p.UsuarioId == usuarioId &&
                    p.Estado == EstadoPrestamo.Activo &&
                    p.Ejemplar != null &&
                    p.Ejemplar.RecursoBibliograficoId == recursoBibliograficoId
                );
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al verificar préstamo activo de recurso: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosVencidosAsync(DateTime fechaEvaluacion)
        {
            try
            {
                _loggerPrestamo.LogInformation("Obteniendo préstamos activos vencidos a la fecha: {Fecha}", fechaEvaluacion);
                return await _context.Prestamos
                    .Where(p => p.Estado == EstadoPrestamo.Activo && p.FechaLimite < fechaEvaluacion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al obtener préstamos activos vencidos: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Prestamo?> ObtenerConDetallesAsync(int id)
        {
            try
            {
                _loggerPrestamo.LogInformation("Obteniendo préstamo con detalles para ID: {Id}", id);
                return await _context.Prestamos
                    .Include(p => p.Usuario)
                    .Include(p => p.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                    .FirstOrDefaultAsync(p => p.PrestamoId == id);
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al obtener préstamo con detalles para ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<Prestamo?> ObtenerPorIdAsync(int id)
        {
            try
            {
                _loggerPrestamo.LogInformation("Obteniendo préstamo por ID: {Id}", id);
                return await _context.Prestamos.FindAsync(id);
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al obtener préstamo por ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> ExistePrestamoActivoPorRecursoAsync(int recursoBibliograficoId)
        {
            try
            {
                _loggerPrestamo.LogInformation("Verificando existencia de préstamo activo para recurso ID: {RecursoId}", recursoBibliograficoId);
                return await _context.Prestamos
                    .AsNoTracking()
                    .AnyAsync(p =>
                        p.Estado == EstadoPrestamo.Activo &&
                        p.Ejemplar != null &&
                        p.Ejemplar.RecursoBibliograficoId == recursoBibliograficoId);
            }
            catch (Exception ex)
            {
                _loggerPrestamo.LogError(ex, "Error al verificar existencia de préstamo activo por recurso: {Message}", ex.Message);
                throw;
            }
        }
    }
}