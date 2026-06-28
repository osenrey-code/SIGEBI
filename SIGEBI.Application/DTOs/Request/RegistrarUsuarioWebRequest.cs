

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarUsuarioWebRequest
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PassWord {  get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
    }
}
