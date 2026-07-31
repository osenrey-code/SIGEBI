using SIGEBI.AppEscritorio.Dtos.Usuarios;

namespace SIGEBI.AppEscritorio.Services.Usuario
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ConsultarUsuariosAsync(ConsultarUsuariosFiltroDto filtro);
        Task<UsuarioDto?> RegistrarAsync(RegistrarUsuarioDto request);
        Task<bool> ActualizarAsync(int id, ActualizarUsuarioDto request);
        Task<bool> ActivarAsync(int id);
        Task<bool> DesactivarAsync(int id, DesactivarUsuarioDto request);
        Task<bool> ResetPasswordAdminAsync(int id, ResetearPasswordAdminDto request);
        Task<bool> CambiarMiPasswordAsync(CambiarPasswordDto request);
    }
}
