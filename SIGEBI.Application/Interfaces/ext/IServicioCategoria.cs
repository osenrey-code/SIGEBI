

using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioCategoria
    {
        Task RegistrarCategoriaAsync(CategoriaRequest request);
        Task<IEnumerable<CategoriaResponse>> ListarTodasAsync();

    }
}
