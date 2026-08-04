using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPenalizacion : RepositorioBase<Penalizacion>, IRepositorioPenalizacion
    {
        private readonly ILogger<RepositorioPenalizacion> _loggerPenalizacion;

        public RepositorioPenalizacion(SIGEBIDbContext context, ILogger<RepositorioPenalizacion> logger) : base(context, logger)
        {
            _loggerPenalizacion = logger;
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            try
            {
                _loggerPenalizacion.LogInformation("Obteniendo penalizaciones para el usuario ID: {UsuarioId}", usuarioId);
                return await _context.Penalizaciones
                   .Include(p => p.Usuario)
                   .Include(p => p.Prestamo)
                   .AsNoTracking()
                   .Where(p => p.UsuarioId == usuarioId)
                   .OrderByDescending(p => p.FechaGeneracion)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerPenalizacion.LogError(ex, "Error al obtener penalizaciones por usuario ID {UsuarioId}: {Message}", usuarioId, ex.Message);
                throw;
            }
        }

        public async Task<Penalizacion?> ObtenerActivaPorUsuarioAsync(int usuarioId)
        {
            try
            {
                _loggerPenalizacion.LogInformation("Obteniendo penalización activa para el usuario ID: {UsuarioId}", usuarioId);
                return await _context.Penalizaciones
                   .Include(p => p.Usuario)
                   .Include(p => p.Prestamo)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(p =>
                       p.UsuarioId == usuarioId &&
                       p.Estado == EstadoPenalizacion.Activa);
            }
            catch (Exception ex)
            {
                _loggerPenalizacion.LogError(ex, "Error al obtener penalización activa para usuario ID {UsuarioId}: {Message}", usuarioId, ex.Message);
                throw;
            }
        }

        public async Task<bool> TienePenalizacionActivaAsync(int usuarioId)
        {
            try
            {
                _loggerPenalizacion.LogInformation("Verificando si el usuario ID {UsuarioId} tiene penalización activa.", usuarioId);
                return await _context.Penalizaciones
                    .AnyAsync(p =>
                        p.UsuarioId == usuarioId &&
                        p.Estado == EstadoPenalizacion.Activa);
            }
            catch (Exception ex)
            {
                _loggerPenalizacion.LogError(ex, "Error al verificar penalización activa para usuario ID {UsuarioId}: {Message}", usuarioId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Penalizacion>> ConsultarAsync(
            int? usuarioId,
            int? prestamoId,
            EstadoPenalizacion? estado,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            try
            {
                _loggerPenalizacion.LogInformation("Consultando penalizaciones con filtros.");

                var query = _context.Penalizaciones
                   .Include(p => p.Usuario)
                   .Include(p => p.Prestamo)
                   .AsNoTracking()
                   .AsQueryable();

                if (usuarioId.HasValue && usuarioId.Value > 0)
                {
                    query = query.Where(p => p.UsuarioId == usuarioId.Value);
                }

                if (prestamoId.HasValue && prestamoId.Value > 0)
                {
                    query = query.Where(p => p.PrestamoId == prestamoId.Value);
                }

                if (estado.HasValue)
                {
                    query = query.Where(p => p.Estado == estado.Value);
                }

                if (fechaInicio.HasValue)
                {
                    query = query.Where(p => p.FechaGeneracion >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    query = query.Where(p => p.FechaGeneracion <= fechaFin.Value);
                }

                return await query
                    .OrderByDescending(p => p.FechaGeneracion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerPenalizacion.LogError(ex, "Error al consultar penalizaciones con filtros: {Message}", ex.Message);
                throw;
            }
        }
    }
}