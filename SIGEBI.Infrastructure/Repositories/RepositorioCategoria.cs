using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioCategoria : RepositorioBase<Categoria>, IRepositorioCategoria
    {
        public RepositorioCategoria(SIGEBIDbContext dbc) : base(dbc)
        {

        }
    }
}
