using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPenalizacion : RepositorioBase<Penalizacion>, IRepositorioPenalizacion
    {
        public RepositorioPenalizacion(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.Prestamo)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.FechaGeneracion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerActivasPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.Prestamo)
                .Where(p =>
                    p.UsuarioId == usuarioId &&
                    p.Estado == EstadoPenalizacion.Activa)
                .OrderByDescending(p => p.FechaGeneracion)
                .ToListAsync();
        }

        public async Task<bool> ExisteActivaPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet.AnyAsync(p =>
                p.UsuarioId == usuarioId &&
                p.Estado == EstadoPenalizacion.Activa);
        }

        public async Task<IEnumerable<Penalizacion>> ConsultarAsync(
            int? usuarioId,
            int? prestamoId,
            string? estado)
        {
            var query = _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.Prestamo)
                .AsQueryable();

            if (usuarioId.HasValue)
            {
                query = query.Where(p => p.UsuarioId == usuarioId.Value);
            }

            if (prestamoId.HasValue)
            {
                query = query.Where(p => p.PrestamoId == prestamoId.Value);
            }

            if (!string.IsNullOrWhiteSpace(estado) &&
                Enum.TryParse<EstadoPenalizacion>(estado, true, out var estadoPenalizacion))
            {
                query = query.Where(p => p.Estado == estadoPenalizacion);
            }

            return await query
                .OrderByDescending(p => p.FechaGeneracion)
                .ToListAsync();
        }
    }
}