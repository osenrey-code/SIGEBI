using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioNotificacion : IRepositorioNotificacion
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<Notificacion> _dbSet;

        public RepositorioNotificacion(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Notificacion>();
        }

        public async Task<Notificacion?> ObtenerporIdAsync(object id)
        {
            var notificacionId = Convert.ToInt32(id);

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NotificacionId == notificacionId);
        }

        public async Task AgregarAsync(Notificacion notificacion)
        {
            await _dbSet.AddAsync(notificacion);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> ConsultarAsync(int? usuarioId, string? tipo)
        {
            var query = _dbSet
                .AsNoTracking()
                .AsQueryable();

            if (usuarioId.HasValue && usuarioId.Value > 0)
            {
                query = query.Where(n => n.UsuarioId == usuarioId.Value);
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                if (Enum.TryParse<TipoNotificacion>(
                        tipo,
                        true,
                        out var tipoConvertido))
                {
                    query = query.Where(n => n.Tipo == tipoConvertido);
                }
            }

            return await query
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task<bool> ExisteAsync(int usuarioId, TipoNotificacion tipo, string mensaje)
        {
            return await _dbSet
                .AnyAsync(n =>
                    n.UsuarioId == usuarioId &&
                    n.Tipo == tipo &&
                    n.Mensaje == mensaje);
        }

        public async Task<IEnumerable<Notificacion>> ObtenerTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }
    }
}