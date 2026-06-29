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

        public async Task<RecursoBibliografico?> ObtenerPorIdentificadorAsync(string identificador)
        {
            return await _dbSet.FirstOrDefaultAsync(r =>
                r.Identificador.ToLower() == identificador.Trim().ToLower());
        }

        public async Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(
            string? titulo,
            string? autor,
            string? categoria,
            bool? soloDisponibles)
        {
            var query = _dbSet.AsQueryable();

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
                query = query.Where(r => r.Categoria.Contains(categoria.Trim()));
            }

            if (soloDisponibles == true)
            {
                query = query.Where(r => r.Estado == EstadoRecurso.Disponible);
            }

            return await query.ToListAsync();
        }
    }
}