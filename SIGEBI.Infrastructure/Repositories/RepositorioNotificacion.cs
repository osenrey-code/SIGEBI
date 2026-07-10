using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioNotificacion : RepositorioBase<Notificacion>,IRepositorioNotificacion
    {

        public RepositorioNotificacion(SIGEBIDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(
            int usuarioId)
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> ObtenerNoLeidasPorUsuarioAsync(
            int usuarioId)
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> ObtenerTodoElHistorialAsync()
        {
            return await _context.Notificaciones
                .AsNoTracking()
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }
    }
}