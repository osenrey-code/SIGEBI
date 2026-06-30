
namespace SIGEBI.Application.DTOs.Request
{
    public class ActualizarUsuarioRequest
    {
        public Guid UsuarioEjecutorId { get; set; }
        public Guid UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
    }
}
