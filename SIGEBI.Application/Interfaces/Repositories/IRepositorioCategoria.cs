using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioCategoria : IBaseRepository<Categoria>
    {
        Task<Categoria?> ObtenerPorNombreAsync(string nombre);
    }
}