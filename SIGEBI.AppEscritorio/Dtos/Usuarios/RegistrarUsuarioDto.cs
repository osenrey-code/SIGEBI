
namespace SIGEBI.AppEscritorio.Dtos.Usuarios
{
    public class RegistrarUsuarioDto
    {
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
