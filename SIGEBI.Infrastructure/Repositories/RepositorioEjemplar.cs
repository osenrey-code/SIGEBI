using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioEjemplar : RepositorioBase<Ejemplar>, IEjemplarRepository
    {

            public RepositorioEjemplar(SIGEBIDbContext context) : base(context)
            {

            }

            public async Task<Ejemplar?> ObtenerPorIdAsync(int id)
            {

                return await _context.Ejemplares
                    .Include(e => e.RecursoBibliografico)
                    .FirstOrDefaultAsync(e => e.EjemplarId == id);
            }

    }
}
