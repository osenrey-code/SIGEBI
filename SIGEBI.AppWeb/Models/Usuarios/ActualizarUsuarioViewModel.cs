namespace SIGEBI.AppWeb.Models.Usuarios
{
    public class ActualizarUsuarioViewModel
    {
        public int UsuarioId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
    }
}
