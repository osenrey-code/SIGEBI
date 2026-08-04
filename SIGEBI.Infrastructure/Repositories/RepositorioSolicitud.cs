using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioSolicitud : RepositorioBase<Solicitud>, ISolicitudRepository
    {
        private readonly ILogger<RepositorioSolicitud> _loggerSolicitud;

        public RepositorioSolicitud(SIGEBIDbContext context, ILogger<RepositorioSolicitud> logger) : base(context, logger)
        {
            _loggerSolicitud = logger;
        }

        public async Task<Solicitud?> ObtenerConDetallesAsync(int id)
        {
            try
            {
                _loggerSolicitud.LogInformation("Obteniendo solicitud con detalles para ID: {Id}", id);
                return await _context.Solicitudes
                    .Include(s => s.Usuario)
                    .Include(s => s.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                    .FirstOrDefaultAsync(s => s.SolicitudId == id);
            }
            catch (Exception ex)
            {
                _loggerSolicitud.LogError(ex, "Error al obtener solicitud con detalles para ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Solicitud>> ObtenerPendientesAsync()
        {
            try
            {
                _loggerSolicitud.LogInformation("Obteniendo solicitudes pendientes.");
                return await _context.Solicitudes
                    .AsNoTracking()
                    .Include(s => s.Usuario)
                    .Include(s => s.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                    .Where(s => s.Estado == EstadoSolicitud.Pendiente)
                    .OrderByDescending(s => s.FechaSolicitud)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerSolicitud.LogError(ex, "Error al obtener solicitudes pendientes: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Solicitud?> ObtenerPorIdAsync(int id)
        {
            try
            {
                _loggerSolicitud.LogInformation("Obteniendo solicitud por ID: {Id}", id);
                return await _context.Solicitudes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SolicitudId == id);
            }
            catch (Exception ex)
            {
                _loggerSolicitud.LogError(ex, "Error al obtener solicitud por ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Solicitud>> ObtenerTodasAsync()
        {
            try
            {
                _loggerSolicitud.LogInformation("Obteniendo todas las solicitudes.");
                return await _context.Solicitudes
                    .AsNoTracking()
                    .Include(s => s.Usuario)
                    .Include(s => s.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                    .OrderByDescending(s => s.FechaSolicitud)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerSolicitud.LogError(ex, "Error al obtener todas las solicitudes: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> ExisteSolicitudPendienteOActivaAsync(int usuarioId, int ejemplarId)
        {
            try
            {
                _loggerSolicitud.LogInformation("Verificando solicitud pendiente o activo para usuario {UsuarioId} y ejemplar {EjemplarId}", usuarioId, ejemplarId);

                bool tieneSolicitudPendiente = await _dbSet.AnyAsync(s =>
                    s.UsuarioId == usuarioId &&
                    s.EjemplarId == ejemplarId &&
                    s.Estado == EstadoSolicitud.Pendiente
                );

                if (tieneSolicitudPendiente)
                    return true;

                bool tienePrestamoActivo = await _context.Set<Prestamo>().AnyAsync(p =>
                    p.UsuarioId == usuarioId &&
                    p.EjemplarId == ejemplarId &&
                    p.Estado == EstadoPrestamo.Activo
                );

                return tienePrestamoActivo;
            }
            catch (Exception ex)
            {
                _loggerSolicitud.LogError(ex, "Error al verificar solicitud pendiente o activa: {Message}", ex.Message);
                throw;
            }
        }
    }
}