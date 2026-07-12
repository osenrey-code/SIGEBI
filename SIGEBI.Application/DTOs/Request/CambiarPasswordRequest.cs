

namespace SIGEBI.Application.DTOs.Request
{
    public record CambiarPasswordRequest
    {
        public int UsuarioId { get; init; }

        public string PasswordActual { get; init; } = string.Empty;

        public string PasswordNueva { get; init; } = string.Empty;

        public string ConfirmarPasswordNueva { get; init; } = string.Empty;
    }
}
