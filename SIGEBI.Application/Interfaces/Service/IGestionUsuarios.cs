using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IGestionUsuariosUseCase
    {
        Task<UsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request, int UsuarioId);

        Task<UsuarioResponse> ActualizarUsuarioAsync(ActualizarUsuarioRequest request, int usuarioId, int actorId);

        Task DesactivarUsuarioAsync(DesactivarUsuarioRequest request, int usuarioId,int actorId);

        Task<IEnumerable<UsuarioResponse>> ConsultarUsuariosAsync(ConsultarUsuariosRequest filtros);
        Task ActivarUsuarioAsync(int usuarioId, int actorId);
        Task CambiarPasswordAsync(CambiarPasswordRequest request, int usuarioId);
        Task<UsuarioResponse> BuscarPorIdAsync(int usuarioId);
        Task<UsuarioResponse> BuscarPorIdentificacionAsync(string identificacion);
    }
}