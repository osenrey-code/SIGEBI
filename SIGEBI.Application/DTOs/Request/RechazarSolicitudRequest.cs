using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record RechazarSolicitudRequest
    {
        [Required(ErrorMessage = "El identificador de la solicitud es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador de la solicitud debe ser mayor a cero.")]
        public int SolicitudId { get; init; }

        [Required(ErrorMessage = "Debe proporcionar un motivo para el rechazo.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 255 caracteres.")]
        public string Motivo { get; init; } = string.Empty;
    }
}
