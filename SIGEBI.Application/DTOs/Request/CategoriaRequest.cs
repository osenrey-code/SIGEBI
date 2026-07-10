
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record CategoriaRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; init; } = string.Empty;
        public string? Descripcion { get; init; } 

    }
}
