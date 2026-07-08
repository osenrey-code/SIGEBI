

using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record AprobarSolicitudRequest
    {
        [Required(ErrorMessage = "El identificador de la solicitud es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la solicitud debe ser mayor a cero.")]
        public int SolicitudId { get; init; }
    }
}
