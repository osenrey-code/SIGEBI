using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.DTOs.Response;
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
            return await _context.Penalizaciones
               .Include(p => p.Usuario)
               .Include(p => p.Prestamo)
               .AsNoTracking()
               .Where(p => p.UsuarioId == usuarioId)
               .OrderByDescending(p => p.FechaGeneracion)
               .ToListAsync();
        }

        public async Task<Penalizacion?> ObtenerActivaPorUsuarioAsync(int usuarioId)
        {
            return await _context.Penalizaciones
           .Include(p => p.Usuario)
           .Include(p => p.Prestamo)
           .AsNoTracking()
           .FirstOrDefaultAsync(p =>
               p.UsuarioId == usuarioId &&
               p.Estado == EstadoPenalizacion.Activa);
        }

        public async Task<bool> TienePenalizacionActivaAsync(int usuarioId)
        {
            return await _context.Penalizaciones
                .AnyAsync(p =>
                    p.UsuarioId == usuarioId &&
                    p.Estado == EstadoPenalizacion.Activa);
        }

        public async Task<IEnumerable<Penalizacion>> ConsultarAsync(
            int? usuarioId,
            int? prestamoId,
            EstadoPenalizacion? estado,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
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

            if (prestamoId.HasValue)
                query = query.Where(p => p.PrestamoId == prestamoId.Value);

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