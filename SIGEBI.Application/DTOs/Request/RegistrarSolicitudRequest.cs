
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record RegistrarSolicitudRequest
    {
        [Required(ErrorMessage = "El identificador del ejemplar es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del ejemplar debe ser mayor a cero.")]
        public int EjemplarId { get; init; }
    }
}
