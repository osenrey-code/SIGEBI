using SIGEBI.AppEscritorio.Dtos.Auth;

namespace SIGEBI.AppEscritorio.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> IniciarSesionAsync(string identificacion, string password);
    }
}
