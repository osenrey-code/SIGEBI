using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPrestamo : RepositorioBase<Prestamo>, IRepositorioPrestamo
    {
        public RepositorioPrestamo(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.Libros)
                .Where(p =>
                    p.UsuarioId == usuarioId &&
                    p.Estado == EstadoPrestamo.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> ObtenerHistorialPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.Libros)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.FechaInicio)
                .ToListAsync();
        }

        public async Task<Prestamo?> ObtenerConDetalleAsync(int prestamoId)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.Libros)
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
        }
    }
}