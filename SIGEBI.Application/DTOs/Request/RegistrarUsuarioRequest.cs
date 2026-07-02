
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarUsuarioRequest
    {
        [Required(ErrorMessage = "El id del usuario es obligatorio.")]
        public int UsuarioEjecutorId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public string Matricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "la contraseña es obligatoria.")]
        public string PassWord { get; set; } = string.Empty;

        public string TipoUsuario { get; set; } = string.Empty;
    }
}
