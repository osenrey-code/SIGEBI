namespace SIGEBI.AppWeb.Models.Usuarios
{
    public class UsuarioItemViewModel
    {
        public int UsuarioId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
