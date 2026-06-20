using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioUsuario : IUsuario 
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<Usuario> _dbSet;

        public RepositorioUsuario(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Usuario>();
        }

        // --- IReaderRepository (Lectura) ---
        public async Task<Usuario?> ObtenerPorIdAsync(object id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync() => await _dbSet.ToListAsync();

        // --- IWriterRepository (Escritura) ---
        public async Task AgregarAsync(Usuario entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Usuario entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Usuario entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        // --- IRepositorioUsuario (Específicos) ---
        public async Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Identificacion == identificacion);
        }

        public async Task<Usuario?> ObtenerConPerfilAsync(Guid id)
        {
            // Usamos Include para cargar el perfil lector asociado (RF-USU-01)
            return await _dbSet
                .Include(u => u.PerfilLector)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
