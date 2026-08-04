using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioNotificacion : RepositorioBase<Notificacion>, IRepositorioNotificacion
    {
        private readonly ILogger<RepositorioNotificacion> _loggerNotificacion;

        public RepositorioNotificacion(SIGEBIDbContext context, ILogger<RepositorioNotificacion> logger) : base(context, logger)
        {
            _loggerNotificacion = logger;
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            try
            {
                _loggerNotificacion.LogInformation("Obteniendo notificaciones para el usuario ID: {UsuarioId}", usuarioId);
                return await _context.Notificaciones
                    .AsNoTracking()
                    .Where(n => n.UsuarioId == usuarioId)
                    .OrderByDescending(n => n.FechaRegistro)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerNotificacion.LogError(ex, "Error al obtener notificaciones para el usuario ID {UsuarioId}: {Message}", usuarioId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Notificacion>> ObtenerNoLeidasPorUsuarioAsync(int usuarioId)
        {
            try
            {
                _loggerNotificacion.LogInformation("Obteniendo notificaciones no leídas para el usuario ID: {UsuarioId}", usuarioId);
                return await _context.Notificaciones
                    .AsNoTracking()
                    .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                    .OrderByDescending(n => n.FechaRegistro)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerNotificacion.LogError(ex, "Error al obtener notificaciones no leídas para usuario ID {UsuarioId}: {Message}", usuarioId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Notificacion>> ObtenerTodoElHistorialAsync()
        {
            try
            {
                _loggerNotificacion.LogInformation("Obteniendo todo el historial de notificaciones.");
                return await _context.Notificaciones
                    .AsNoTracking()
                    .OrderByDescending(n => n.FechaRegistro)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerNotificacion.LogError(ex, "Error al obtener el historial de notificaciones: {Message}", ex.Message);
                throw;
            }
        }
    }
}