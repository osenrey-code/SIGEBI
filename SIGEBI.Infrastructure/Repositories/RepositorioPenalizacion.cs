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
            return await _context.Penalizaciones
                .Include(p => p.Prestamo)
                .AsNoTracking()
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Penalizacion?> ObtenerActivaPorUsuarioAsync(int usuarioId)
        {
            return await _context.Penalizaciones
                .Include(p => p.Prestamo)
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
            EstadoPenalizacion? estado,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _context.Penalizaciones
                .Include(p => p.Usuario)
                .Include(p => p.Prestamo)
                .AsNoTracking() 
                .AsQueryable();

            if (usuarioId > 0)
            {
                query = query.Where(p => p.UsuarioId == usuarioId);
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

            return await query.ToListAsync();
        }
    }
}