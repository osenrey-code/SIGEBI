using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioSolicitud : RepositorioBase<Solicitud>, ISolicitudRepository
    {
        public RepositorioSolicitud(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<Solicitud?> ObtenerConDetallesAsync(int id)
        {
            return await _context.Solicitudes
                .Include(s => s.Usuario)
                .Include(s => s.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                .FirstOrDefaultAsync(s => s.SolicitudId == id);
        }

        public async Task<IEnumerable<Solicitud>> ObtenerPendientesAsync()
        {
            return await _context.Solicitudes
                .AsNoTracking()
                .Include(s => s.Usuario)
                .Include(s => s.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                .Where(s => s.Estado == EstadoSolicitud.Pendiente)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<Solicitud?> ObtenerPorIdAsync(int id)
        {
            return await _context.Solicitudes
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SolicitudId == id);
        }

        public async Task<IEnumerable<Solicitud>> ObtenerTodasAsync()
        {
            return await _context.Solicitudes
                .AsNoTracking()
                .Include(s => s.Usuario)
                .Include(s => s.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<bool> ExisteSolicitudPendienteOActivaAsync(int usuarioId, int ejemplarId)
        {
            var recursoId = await _context.Ejemplares
            .Where(e => e.EjemplarId == ejemplarId)
            .Select(e => e.RecursoBibliograficoId)
            .FirstOrDefaultAsync();

            if (recursoId == 0) return false;

            return await _dbSet.AnyAsync(s =>
                s.UsuarioId == usuarioId &&
                s.Ejemplar != null &&
                s.Ejemplar.RecursoBibliograficoId == recursoId &&
                (s.Estado == EstadoSolicitud.Pendiente || s.Estado == EstadoSolicitud.Aprobada)
            );
        }
    }
}