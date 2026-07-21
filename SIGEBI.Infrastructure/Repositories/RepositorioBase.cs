using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioBase<T> : IBaseRepository<T> where T : class
    {
        protected readonly SIGEBIDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // Constructor public para que se pueda inyectar en la aplicación
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
        }

        public Task ActualizarAsync(T entidad)
        {
            _dbSet.Update(entidad);
            return Task.CompletedTask;
        }

        public  Task EliminarAsync(T entidad)
        {
            _dbSet.Remove(entidad);
            return Task.CompletedTask;
        }

        public async Task<T?> ObtenerporIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}

