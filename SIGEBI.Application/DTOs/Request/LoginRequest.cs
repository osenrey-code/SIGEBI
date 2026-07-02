using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record LoginRequest
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; init; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; init; } = string.Empty;
    }
}
