using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPrestamo : RepositorioBase<Prestamo>, IRepositorioPrestamo
    {
        public RepositorioPrestamo(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Prestamo>> ConsultarActivosAsync(int? usuarioId, int? ejemplarId)
        {
            var query = _context.Prestamos
                .AsNoTracking()
                .Include(p => p.Ejemplar)
                   .ThenInclude(e => e!.RecursoBibliografico)
                .Where(p => p.Estado == EstadoPrestamo.Activo)
                .AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(p => p.UsuarioId == usuarioId.Value);

            if (ejemplarId.HasValue)
                query = query.Where(p => p.EjemplarId == ejemplarId.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> ConsultarHistorialAsync(int? usuarioId, int? ejemplarId)
        {
            var query = _context.Prestamos
                .AsNoTracking()
                .Include(p => p.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                .AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(p => p.UsuarioId == usuarioId.Value);

            if (ejemplarId.HasValue)
                query = query.Where(p => p.EjemplarId == ejemplarId.Value);

            query = query.OrderByDescending(p => p.FechaInicio);

            return await query.ToListAsync();
        }

      

        public async Task<int> ContarActivosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Prestamos
                .CountAsync(p => p.UsuarioId == usuarioId && p.Estado == EstadoPrestamo.Activo);
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Prestamos
                .AsNoTracking()
                .Where(p => p.UsuarioId == usuarioId && p.Estado == EstadoPrestamo.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosVencidosAsync(DateTime fechaEvaluacion)
        {
            return await _context.Prestamos
                .Where(p => p.Estado == EstadoPrestamo.Activo && p.FechaLimite < fechaEvaluacion)
                .ToListAsync();
        }

        public async Task<Prestamo?> ObtenerConDetallesAsync(int id)
        {
            return await _context.Prestamos
            .Include(p => p.Usuario)
            .Include(p => p.Ejemplar)
                .ThenInclude(e => e!.RecursoBibliografico)
            .FirstOrDefaultAsync(p => p.PrestamoId == id);
        }

        public async Task<ReportePrestamoResponse> ObtenerEstadisticaPrestamoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var query = _dbSet
        .Include(p => p.Devolucion)
        .AsNoTracking()
        .Where(p => p.FechaInicio >= fechaInicio &&
                    p.FechaInicio <= fechaFin);

            int total = await query.CountAsync();

            int puntuales = await query.CountAsync(p =>
                p.Devolucion != null &&
                p.Devolucion.FechaDevolucion <= p.FechaLimite);

            int vencidos = await query.CountAsync(p =>
                (p.Devolucion != null &&
                 p.Devolucion.FechaDevolucion > p.FechaLimite)
                ||
                (p.Devolucion == null &&
                 DateTime.UtcNow > p.FechaLimite));

            decimal tasa = total > 0
                ? Math.Round((decimal)puntuales / total * 100, 2)
                : 0;

            return new ReportePrestamoResponse
            {
                TotalPrestamos = total,
                DevolucionesPuntuales = puntuales,
                PrestamosVencidos = vencidos,
                TasaDevolucionPuntual = tasa
            };
        }

        public async Task<Prestamo?> ObtenerPorIdAsync(int id)
        {
            return await _context.Prestamos.FindAsync(id);
        }

        public async Task<bool> ExistePrestamoActivoPorRecursoAsync(
        int recursoBibliograficoId)
        {
            return await _context.Prestamos
                .AsNoTracking()
                .AnyAsync(p =>
                    p.Estado == EstadoPrestamo.Activo &&
                    p.Ejemplar != null &&
                    p.Ejemplar.RecursoBibliograficoId == recursoBibliograficoId);
        }
    }
}
