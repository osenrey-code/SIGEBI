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

        public async Task<IReadOnlyList<Notificacion>> ObtenerTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task AgregarAsync(Notificacion entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public Task ActualizarAsync(Notificacion entidad)
        {
            throw new NotSupportedException(
                "Las notificaciones no pueden ser modificadas."
            );
        }

        public async Task<IEnumerable<Notificacion>> ConsultarAsync(
            int? usuarioId,
            TipoNotificacion? tipo,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet
                .AsNoTracking()
                .AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(n => n.UsuarioId == usuarioId.Value);

            if (tipo.HasValue)
                query = query.Where(n => n.Tipo == tipo.Value);

            if (fechaInicio.HasValue)
                query = query.Where(n => n.FechaRegistro >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(n => n.FechaRegistro <= fechaFin.Value);

            return await query
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }
    }
}