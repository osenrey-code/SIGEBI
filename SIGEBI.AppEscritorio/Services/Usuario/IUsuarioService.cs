using SIGEBI.AppEscritorio.Dtos.Usuarios;

namespace SIGEBI.AppEscritorio.Services.Usuario
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ConsultarUsuariosAsync(ConsultarUsuariosFiltroDto filtro);
        Task<UsuarioDto?> RegistrarAsync(RegistrarUsuarioDto request);
        Task<bool> Actualizar(int id, ActualizarUsuarioDto request);
        Task<bool> Activar(int id);
        Task<bool> Desactivar(int id, DesactivarUsuarioDto request);
        Task<bool> ResetPasswordAdminAsync(int id, ResetearPasswordAdminDto request);
        Task<bool> CambiarMiPasswordAsync(CambiarPasswordDto request);
    }
}
