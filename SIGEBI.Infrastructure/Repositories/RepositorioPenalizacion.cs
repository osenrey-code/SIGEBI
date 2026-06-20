using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPenalizacion : IRepositorioPenalizacion 
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<Penalizacion> _dbSet;

        public RepositorioPenalizacion(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Penalizacion>();
        }

        // --- IReaderRepository ---
        public async Task<Penalizacion?> ObtenerPorIdAsync(object id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<Penalizacion>> ObtenerTodosAsync() => await _dbSet.ToListAsync();

        // --- IWriterRepository ---
        public async Task AgregarAsync(Penalizacion entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Penalizacion entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Penalizacion entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        // --- IRepositorioPenalizacion (Específicos ajustados al nombre de tu entidad) ---

        public async Task<IEnumerable<Penalizacion>> ObtenerPorPerfilLectorAsync(Guid perfilLectorId)
        {
            return await _dbSet
                .Where(p => p.perfilLectorId == perfilLectorId)
                // Ajustado al nombre exacto de tu entidad
                .ToListAsync();
        }

        public async Task<Penalizacion?> ObtenerActivaPorPerfilLectorAsync(Guid perfilLectorId)
        {
            // Usamos tu Enum EstadoPenalizacion.Activa y la propiedad Estado de tu entidad
            return await _dbSet
                .FirstOrDefaultAsync(p => p.perfilLectorId == perfilLectorId
                                      && p.Estado == SIGEBI.Domain.Enums.EstadoPenalizacion.Activa);
        }
    }
}
