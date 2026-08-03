namespace SIGEBI.AppWeb.Models.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
