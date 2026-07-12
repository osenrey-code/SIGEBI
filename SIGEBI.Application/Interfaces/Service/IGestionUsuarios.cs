using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IGestionUsuariosUseCase
    {
        Task<UsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request, int UsuarioId);

        Task<UsuarioResponse> ActualizarUsuarioAsync(ActualizarUsuarioRequest request, int actorId);

        Task DesactivarUsuarioAsync(DesactivarUsuarioRequest request,int actorId);

        Task<IEnumerable<UsuarioResponse>> ConsultarUsuariosAsync(ConsultarUsuariosRequest filtros);

    }
}