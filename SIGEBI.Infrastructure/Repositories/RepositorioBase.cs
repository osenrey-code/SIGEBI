using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioBase<T> : IBaseRepository<T> where T : class
    {
        protected readonly SIGEBIDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<RepositorioBase<T>> _logger;

        public RepositorioBase(SIGEBIDbContext context, ILogger<RepositorioBase<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los registros de la entidad {Entity}", typeof(T).Name);
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de {Entity}: {Message}", typeof(T).Name, ex.Message);
                throw;
            }
        }

        public async Task AgregarAsync(T entidad)
        {
            try
            {
                _logger.LogInformation("Agregando un nuevo registro a la entidad {Entity}", typeof(T).Name);
                await _dbSet.AddAsync(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar registro en {Entity}: {Message}", typeof(T).Name, ex.Message);
                throw;
            }
        }

        public Task ActualizarAsync(T entidad)
        {
            try
            {
                _logger.LogInformation("Actualizando registro en la entidad {Entity}", typeof(T).Name);
                _dbSet.Update(entidad);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar registro en {Entity}: {Message}", typeof(T).Name, ex.Message);
                throw;
            }
        }

        public Task EliminarAsync(T entidad)
        {
            try
            {
                _logger.LogInformation("Eliminando registro de la entidad {Entity}", typeof(T).Name);
                _dbSet.Remove(entidad);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar registro en {Entity}: {Message}", typeof(T).Name, ex.Message);
                throw;
            }
        }

        public async Task<T?> ObtenerporIdAsync(object id)
        {
            try
            {
                _logger.LogInformation("Buscando registro en {Entity} con ID: {Id}", typeof(T).Name, id);
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar registro en {Entity} con ID {Id}: {Message}", typeof(T).Name, id, ex.Message);
                throw;
            }
        }
    }
}