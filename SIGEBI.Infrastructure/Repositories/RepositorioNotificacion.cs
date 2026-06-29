using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
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

        public async Task<Notificacion?> ObtenerPorIdAsync(object id)
        {
            return await _dbSet.FirstOrDefaultAsync(n => n.Id == (Guid)id);
        }

        public async Task<IEnumerable<Notificacion>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AgregarAsync(Notificacion entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Notificacion entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Notificacion entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notificacion>> ConsultarAsync(
            Guid? usuarioDestinatarioId,
            string? tipoEvento,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet.AsQueryable();

            if (usuarioDestinatarioId.HasValue)
            {
                query = query.Where(n => n.UsuarioDestinatarioId == usuarioDestinatarioId.Value);
            }

            if (!string.IsNullOrWhiteSpace(tipoEvento))
            {
                query = query.Where(n => n.TipoEvento.Contains(tipoEvento.Trim()));
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(n => n.FechaRegistro >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(n => n.FechaRegistro <= fechaFin.Value);
            }

            return await query.ToListAsync();
        }
    }
}