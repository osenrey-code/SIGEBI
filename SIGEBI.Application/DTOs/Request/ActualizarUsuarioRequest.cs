
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record ActualizarUsuarioRequest
    {

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        public string NombreCompleto { get; set; } = string.Empty;
    
    }
}
