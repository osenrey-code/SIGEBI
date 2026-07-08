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

        public async Task<ReportePenalizacionesResponse> ObtenerEstadisticaPenalizacionesAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var query = _context.Penalizaciones
       .AsNoTracking()
       .Where(p => p.FechaGeneracion >= fechaInicio &&
                   p.FechaGeneracion <= fechaFin);

            int total = await query.CountAsync();

            int activas = await query.CountAsync(p =>
                p.Estado == EstadoPenalizacion.Activa);

            int resueltas = await query.CountAsync(p =>
                p.Estado == EstadoPenalizacion.Pagada);

            int totalDiasRetraso = await query.SumAsync(p =>
                (int?)p.DiasRetraso) ?? 0;

            decimal montoTotal = await query.SumAsync(p =>
                (decimal?)p.MontoMora) ?? 0;

            decimal montoActivo = await query
                .Where(p => p.Estado == EstadoPenalizacion.Activa)
                .SumAsync(p => (decimal?)p.MontoMora) ?? 0;

            decimal montoResuelto = await query
                .Where(p => p.Estado == EstadoPenalizacion.Pagada)
                .SumAsync(p => (decimal?)p.MontoMora) ?? 0;

            return new ReportePenalizacionesResponse
            {
                TotalPenalizaciones = total,
                PenalizacionesActivas = activas,
                PenalizacionesResueltas = resueltas,
                TotalDiasRetraso = totalDiasRetraso,
                MontoTotalMora = montoTotal,
                MontoMoraActiva = montoActivo,
                MontoMoraResuelta = montoResuelto
            };
        }
    }
}