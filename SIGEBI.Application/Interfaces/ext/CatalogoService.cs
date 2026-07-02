

using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface CatalogoService
    {
        Task RegistrarRecursoAsync(RegistrarRecursoRequest request, int UsuarioEjecutorId);
        Task<RecursoResponse?> BuscarPorIsbnAsync(string isbn);
        Task<IEnumerable<RecursoResponse?>> ConsultarTodosAsync();
    }
}
