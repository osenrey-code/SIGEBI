
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record RegistrarUsuarioRequest
    {

        [Required(ErrorMessage = "La identificación (Matrícula/Código) es obligatoria.")]
        public string Identificacion { get; init; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string NombreCompleto { get; init; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Correo { get; init; } = string.Empty;

        [Required(ErrorMessage = "El tipo de usuario es obligatorio.")]
        public string Tipo { get; init; } = string.Empty;

    }
}
