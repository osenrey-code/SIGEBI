using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioDevolucion : RepositorioBase<Devolucion>, IRepositorioDevolucion
    {
        public RepositorioDevolucion(SIGEBIDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Devolucion>> ConsultarHistorialAsync(int? usuarioId, int? ejemplarId, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(d => d.Prestamo)
                    .ThenInclude(p => p!.Usuario)
                .Include(d => d.Prestamo)
                    .ThenInclude(p => p!.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                .AsQueryable();

            if (usuarioId.HasValue)
            {
                query = query.Where(d =>
                    d.Prestamo != null &&
                    d.Prestamo.UsuarioId == usuarioId.Value);
            }

            if (ejemplarId.HasValue)
            {
                query = query.Where(d =>
                    d.Prestamo != null &&
                    d.Prestamo.EjemplarId == ejemplarId.Value);
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(d =>
                    d.FechaDevolucion >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(d =>
                    d.FechaDevolucion <= fechaFin.Value);
            }

            return await query
                .OrderByDescending(d => d.FechaDevolucion)
                .ToListAsync();
        }
        

        public async Task<Devolucion?> ObtenerPorIdAsync(int devolucionId)
        {
            return await _context.Devolucion
               .AsNoTracking()
               .Include(d => d.Prestamo)
                   .ThenInclude(p => p!.Usuario)
               .Include(d => d.Prestamo)
                   .ThenInclude(p => p!.Ejemplar)
                       .ThenInclude(e => e!.RecursoBibliografico)
               .FirstOrDefaultAsync(d => d.DevolucionId == devolucionId);

        }

        public async Task<Devolucion?> ObtenerPorPrestamoIdAsync(int prestamoId)
        {
            return await _context.Devolucion
                .AsNoTracking()
                .Include(d => d.Prestamo)
                    .ThenInclude(p => p!.Usuario)
                .Include(d => d.Prestamo)
                    .ThenInclude(p => p!.Ejemplar)
                        .ThenInclude(e => e!.RecursoBibliografico)
                .FirstOrDefaultAsync(d => d.PrestamoId == prestamoId);
        }
    }
}
