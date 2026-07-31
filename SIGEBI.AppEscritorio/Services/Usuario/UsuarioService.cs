using SIGEBI.AppEscritorio.Dtos.Usuarios;
using SIGEBI.AppEscritorio.Services.Api;

namespace SIGEBI.AppEscritorio.Services.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IApiClient _apiClient;

        public UsuarioService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<bool> Activar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Actualizar(int id, ActualizarUsuarioDto request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CambiarMiPasswordAsync(CambiarPasswordDto request)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioDto>> ConsultarUsuariosAsync(ConsultarUsuariosFiltroDto filtro)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Desactivar(int id, DesactivarUsuarioDto request)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioDto?> RegistrarAsync(RegistrarUsuarioDto request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPasswordAdminAsync(int id, ResetearPasswordAdminDto request)
        {
            throw new NotImplementedException();
        }
    }
}
