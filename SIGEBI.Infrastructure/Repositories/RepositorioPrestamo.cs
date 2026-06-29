using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPrestamo : RepositorioBase<Prestamo>, IRepositorioPrestamo
    {
        public RepositorioPrestamo(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(Guid usuarioId)
        {
            return await _dbSet
                .Where(p =>
                    p.PerfilLectorId == usuarioId &&
                    p.Estado == EstadoPrestamo.Activo)
                .ToListAsync();
        }

        public async Task<int> ContarActivosPorUsuarioAsync(Guid usuarioId)
        {
            return await _dbSet
                .CountAsync(p =>
                    p.PerfilLectorId == usuarioId &&
                    p.Estado == EstadoPrestamo.Activo);
        }

        public async Task<IEnumerable<Prestamo>> ObtenerHistorialPorUsuarioAsync(Guid usuarioId)
        {
            return await _dbSet
                .Where(p => p.PerfilLectorId == usuarioId)
                .OrderByDescending(p => p.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> ObtenerPrestamosProximosAVencerAsync(
        DateTime fechaDesde,
        DateTime fechaHasta)
        {
            return await _dbSet
                .Include(p => p.PerfilLector)
                .Include(p => p.Recurso)
                .Where(p =>
                    p.Estado == EstadoPrestamo.Activo &&
                    p.FechaLimite.HasValue &&
                    p.FechaLimite.Value.Date >= fechaDesde.Date &&
                    p.FechaLimite.Value.Date <= fechaHasta.Date)
                .ToListAsync();
        }
    }
}
