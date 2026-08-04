using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioRecurso : RepositorioBase<RecursoBibliografico>, IRepositorioRecurso
    {
        private readonly ILogger<RepositorioRecurso> _loggerRecurso;

        public RepositorioRecurso(SIGEBIDbContext context, ILogger<RepositorioRecurso> logger) : base(context, logger)
        {
            _loggerRecurso = logger;
        }

        public override async Task<IEnumerable<RecursoBibliografico>> ObtenerTodosAsync()
        {
            try
            {
                _loggerRecurso.LogInformation("Obteniendo todos los recursos bibliográficos activos con categoría y ejemplares.");
                return await _dbSet
                    .Include(r => r.Categoria)
                    .Include(r => r.Ejemplares)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerRecurso.LogError(ex, "Error al obtener todos los recursos bibliográficos: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<RecursoBibliografico?> BuscarPorIsbnAsync(string isbn)
        {
            try
            {
                _loggerRecurso.LogInformation("Buscando recurso bibliográfico por ISBN (incluyendo inactivos): {Isbn}", isbn);
                return await _dbSet
                    .IgnoreQueryFilters() 
                    .Include(r => r.Categoria)
                    .Include(r => r.Ejemplares)
                    .FirstOrDefaultAsync(r => r.ISBN.ToLower() == isbn.Trim().ToLower());
            }
            catch (Exception ex)
            {
                _loggerRecurso.LogError(ex, "Error al buscar recurso por ISBN {Isbn}: {Message}", isbn, ex.Message);
                throw;
            }
        }

        public async Task<RecursoBibliografico?> BuscarConCategoriaAsync(int recursoBibliograficoId)
        {
            try
            {
                _loggerRecurso.LogInformation("Buscando recurso con categoría para ID: {Id}", recursoBibliograficoId);
                return await _dbSet
                    .Include(r => r.Categoria)
                    .Include(r => r.Ejemplares)
                    .FirstOrDefaultAsync(r => r.RecursoBibliograficoId == recursoBibliograficoId);
            }
            catch (Exception ex)
            {
                _loggerRecurso.LogError(ex, "Error al buscar recurso con categoría por ID {Id}: {Message}", recursoBibliograficoId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(
            string? titulo,
            string? autor,
            string? categoria,
            bool? soloDisponibles)
        {
            try
            {
                _loggerRecurso.LogInformation("Consultando catálogo de recursos activos con filtros.");

                var query = _dbSet
                    .Include(r => r.Categoria)
                    .Include(r => r.Ejemplares)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(titulo))
                {
                    query = query.Where(r => r.Titulo.Contains(titulo.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(autor))
                {
                    query = query.Where(r => r.Autor.Contains(autor.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(categoria))
                {
                    query = query.Where(r =>
                        r.Categoria != null &&
                        r.Categoria.Nombre.Contains(categoria.Trim()));
                }

                if (soloDisponibles == true)
                {
                    query = query.Where(r =>
                        r.Ejemplares.Any(e => e.Estado == EstadoEjemplar.Disponible));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _loggerRecurso.LogError(ex, "Error al consultar el catálogo de recursos: {Message}", ex.Message);
                throw;
            }
        }
    }
}