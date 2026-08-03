

namespace SIGEBI.Application.DTOs.Request
{
    public record CambiarPasswordRequest
    {
        public string PasswordActual { get; init; } = string.Empty;

        public string PasswordNueva { get; init; } = string.Empty;

        public string ConfirmarPasswordNueva { get; init; } = string.Empty;
    }
}
