using SIGEBI.AppEscritorio.Dtos.Auth;
using SIGEBI.AppEscritorio.Services.Implementaciones;
using SIGEBI.AppEscritorio.Session;

namespace SIGEBI.AppEscritorio.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IApiClient _apiClient;

        public AuthService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AuthResult> IniciarSesionAsync(string identificacion, string password)
        {
            if (string.IsNullOrEmpty(identificacion))
                return AuthResult.Fallo("El código de identificación es obligatorio.");

            if (string.IsNullOrWhiteSpace(password))
                return AuthResult.Fallo("La contraseña es obligatoria.");

            try
            {
                var request = new LoginRequestDto
                {
                    Identificacion = identificacion,
                    Password = password
                };

                var response = await _apiClient.PostAsync<LoginRequestDto, LoginResponseDto>("api/account/login", request);

                if (response == null || string.IsNullOrEmpty(response.Token))
                {
                    return AuthResult.Fallo("Respuesta de autenticación no válida emitida por el servidor.");
                }

                UserSession.Instancia.IniciarSesion(
                    response.UsuarioId,
                    response.Identificacion,
                    response.NombreCompleto,
                    response.Correo,
                    response.TipoUsuario,
                    response.Token
                    );

                return AuthResult.Ok(response.TipoUsuario);

            } catch(Exception ex)
            {
                return AuthResult.Fallo(ex.Message);
            }
        }
    }
}
