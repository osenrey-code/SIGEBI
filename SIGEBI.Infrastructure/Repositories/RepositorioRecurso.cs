using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioRecurso : RepositorioBase<RecursoBibliografico>, IRepositorioRecurso
    {
        public RepositorioRecurso(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<RecursoBibliografico?> BuscarPorIsbnAsync(string isbn)
        {
            return await _dbSet
                .Include(r => r.Categoria)
                .Include(r => r.Ejemplares)
                .FirstOrDefaultAsync(r => r.ISBN.ToLower() == isbn.Trim().ToLower());
        }

        public async Task<RecursoBibliografico?> BuscarConCategoriaAsync(int recursoBibliograficoId)
        {
            return await _dbSet
                .Include(r => r.Categoria)
                .Include(r => r.Ejemplares)
                .FirstOrDefaultAsync(r => r.RecursoBibliograficoId == recursoBibliograficoId);
        }

        public async Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(
            string? titulo,
            string? autor,
            string? categoria,
            bool? soloDisponibles)
        {
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
    }
}