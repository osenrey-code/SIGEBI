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
            // Asumimos que un préstamo está activo si NO tiene fecha de devolución real
            return await _dbSet
                .Where(p => p.PerfilLectorId == usuarioId && p.FechaDevolucion == null)
                .ToListAsync();
        }

        public async Task<int> ContarActivosPorUsuarioAsync(Guid usuarioId)
        {
            // CountAsync es mucho más rápido y ligero para la BD que traer toda la lista
            return await _dbSet
                .CountAsync(p => p.PerfilLectorId == usuarioId && p.FechaDevolucion == null);
        }

        public async Task<IEnumerable<Prestamo>> ObtenerHistorialPorUsuarioAsync(Guid usuarioId)
        {
            // RF-PRE-06: El historial completo, ordenado por los más recientes primero
            return await _dbSet
                .Where(p => p.PerfilLectorId == usuarioId)
                .OrderByDescending(p => p.FechaInicio)
                .ToListAsync();
        }
    }
}
