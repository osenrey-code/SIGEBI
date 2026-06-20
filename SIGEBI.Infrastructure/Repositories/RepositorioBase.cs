using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioBase<T> : IBaseRepository<T> where T : class
    {
        protected readonly SIGEBIDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // 3. Constructor 'public' para que se pueda inyectar en la aplicación
        public RepositorioBase(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(T entidad)
        {
            // Update no es asíncrono en EF Core, pero SaveChangesAsync sí lo es
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(T entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<T> ObtenerporIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}

