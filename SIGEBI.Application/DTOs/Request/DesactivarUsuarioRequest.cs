using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record DesactivarUsuarioRequest
    {
        [Required(ErrorMessage = "El motivo de la desactivación es obligatorio.")]
        [StringLength(255, ErrorMessage = "El motivo no puede exceder los 255 caracteres.")]
        public string Motivo { get; init; } = string.Empty;
    }
}
