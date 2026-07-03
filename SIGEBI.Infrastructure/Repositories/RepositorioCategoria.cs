using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioCategoria : RepositorioBase<Categoria>, IRepositorioCategoria
    {
        public RepositorioCategoria(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<Categoria?> ObtenerPorNombreAsync(string nombre)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.Trim().ToLower());
        }
    }
}