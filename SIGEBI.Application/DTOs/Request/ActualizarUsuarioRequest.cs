
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record ActualizarUsuarioRequest
    {
        [Required(ErrorMessage = "El ID del usuario Matricula/CodiEmpleado es obligatorio.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        public string NombreCompleto { get; set; } = string.Empty;
    
    }
}
