using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioNotificacion : RepositorioBase<Notificacion>, IRepositorioNotificacion
    {
        public RepositorioNotificacion(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> ConsultarAsync(
            int? usuarioId,
            string? tipo)
        {
            var query = _dbSet.AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(n => n.UsuarioId == usuarioId.Value);

            if (!string.IsNullOrWhiteSpace(tipo) &&
                Enum.TryParse<TipoNotificacion>(tipo, true, out var tipoNotificacion))
            {
                query = query.Where(n => n.Tipo == tipoNotificacion);
            }

            return await query
                .OrderByDescending(n => n.FechaRegistro)
                .ToListAsync();
        }

        public async Task<bool> ExisteAsync(
            int usuarioId,
            TipoNotificacion tipo,
            string mensaje)
        {
            return await _dbSet.AnyAsync(n =>
                n.UsuarioId == usuarioId &&
                n.Tipo == tipo &&
                n.Mensaje == mensaje);
        }
    }
}