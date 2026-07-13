using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service;

public interface IGestionCategorias
{
    Task<CategoriaResponse> RegistrarCategoriaAsync(
        CategoriaRequest request,
        int actorId
    );

    Task<IEnumerable<CategoriaResponse>> ConsultarCategoriasAsync();
}