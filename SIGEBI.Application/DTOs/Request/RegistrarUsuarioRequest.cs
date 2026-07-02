
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarUsuarioRequest
    {
        [Required(ErrorMessage = "El codigo de identificación del usuario es obligatorio.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es necesaria.")]
        public string Identifiacion { get; set; } = string.Empty;


        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de usuario es obligatorio")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "la contraseña es obligatoria.")]
        public string PassWord { get; set; } = string.Empty;

        public string Matricula { get; set; } = string.Empty;
        public string CodigoEmpleado { get; set; } = string.Empty;

    }
}
