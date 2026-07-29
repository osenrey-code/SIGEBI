namespace SIGEBI.AppWeb.Models.DTOs.Usuarios
{
    public class CambiarPasswordRequest
    {
        public string PasswordActual { get; set; } = string.Empty;
        public string PasswordNueva { get; set; } = string.Empty;
        public string ConfirmarPasswordNueva { get; set; } = string.Empty;
    }
}
