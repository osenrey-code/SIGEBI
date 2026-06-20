using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using SIGEBI.Infrastructure.Persistencia;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPrestamo : IRepositorioPrestamo
    {
        private readonly SIGEBIDbContext _context;

        public RepositorioPrestamo(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task Guardar(Prestamo prestamo)
        {
            await _context.Prestamos.AddAsync(prestamo);
        }

        public async Task<Prestamo?> ObtenerPorId(Guid id)
        {
            return await _context.Prestamos
                .Include(p => p.PerfilLector)
                .Include(p => p.Recurso)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Prestamo>> ObtenerPrestamosActivosPorLectorAsync(Guid perfilLectorId)
        {
            return await _context.Prestamos
                .Where(p => p.PerfilLectorId == perfilLectorId &&
                           (p.Estado == EstadoPrestamo.Activo || p.Estado == Domain.Enums.EstadoPrestamo.Vencido))
                .ToListAsync();
        }
    }
}
