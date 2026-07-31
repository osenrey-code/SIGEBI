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

        public async Task<bool> ActivarAsync(int id)
        {
            await _apiClient.PatchAsync($"api/usuarios/{id}/activar", new { });
            return true;
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarUsuarioDto request)
        {
            await _apiClient.PutAsync($"api/usuarios/{id}/actualizar", request);
            return true; 
        }

        public async Task<bool> CambiarMiPasswordAsync(CambiarPasswordDto request)
        {
            await _apiClient.PatchAsync("api/usuarios/cambiar-mi-password", request);
            return true;
        }

        public async Task<List<UsuarioDto>> ConsultarUsuariosAsync(ConsultarUsuariosFiltroDto filtro)
        {
            var queryParams = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                queryParams.Add($"nombre={Uri.EscapeDataString(filtro.Nombre)}");

            if (!string.IsNullOrWhiteSpace(filtro.Identificacion))
                queryParams.Add($"Identificacion={Uri.EscapeDataString(filtro.Identificacion)}");

            if (!string.IsNullOrWhiteSpace(filtro.TipoUsuario) && filtro.TipoUsuario != "Todos")
                queryParams.Add($"TipoUsuario={Uri.EscapeDataString(filtro.TipoUsuario!)}");

            if (!string.IsNullOrWhiteSpace(filtro.Estado) && filtro.Estado != "Todos")
                queryParams.Add($"Estado={Uri.EscapeDataString(filtro.Estado)}");

            string url = "api/usuarios/consultar";
            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            var resultado = await _apiClient.GetTAsync<List<UsuarioDto>>(url);
            return resultado ?? new List<UsuarioDto>();
        }

        public async Task<bool> DesactivarAsync(int id, DesactivarUsuarioDto request)
        {
            await _apiClient.PatchAsync($"api/usuarios/{id}/desactivar", request);
            return true;
        }

        public async Task<UsuarioDto?> RegistrarAsync(RegistrarUsuarioDto request)
        {
            return await _apiClient.PostAsync<RegistrarUsuarioDto, UsuarioDto>("api/usuarios/registrar", request);
        }

        public async Task<bool> ResetPasswordAdminAsync(int id, ResetearPasswordAdminDto request)
        {
            await _apiClient.PatchAsync($"api/usuarios/{id}/resetear-password-admin", request);
            return true;
        }
    }
}
