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
              return await _dbSet.AnyAsync(s =>
              s.UsuarioId == usuarioId &&
              s.EjemplarId == ejemplarId &&
             (s.Estado == EstadoSolicitud.Pendiente || s.Estado == EstadoSolicitud.Aprobada)
              );  
        }
    }
}