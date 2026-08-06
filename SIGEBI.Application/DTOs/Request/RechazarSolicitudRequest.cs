
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public class RechazarSolicitudRequest
    {
        [Required(ErrorMessage = "El ID de la solicitud es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la solicitud debe ser mayor a 0.")]
        public int SolicitudId { get; set; }

        [Required(ErrorMessage = "El motivo de rechazo es obligatorio.")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres.")]
        public string MotivoRechazo { get; set; } = string.Empty;
    }
}
