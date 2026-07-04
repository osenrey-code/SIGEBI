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
                   .ThenInclude(e => e.RecursoBibliografico)
                .FirstAsync(p => p.PrestamoId == id);
        }

        public async Task<Prestamo?> ObtenerPorIdAsync(int id)
        {
            return await _context.Prestamos.FindAsync();
        }
    }
}
