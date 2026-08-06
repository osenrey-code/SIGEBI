using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public record SolicitudResponse
    {
        public int SolicitudId { get; init; }
        public string TituloRecurso { get; init; } = string.Empty;
        public string IdentificadorEjemplar { get; init; } = string.Empty;
        public DateTime FechaSolicitud { get; init; }
        public string Estado { get; init; } = string.Empty;
        public string? MotivoRechazo { get; init; }

        public string NombreUsuario { get; set; } = string.Empty;
        public string IdentificacionUsuario { get; set; } = string.Empty;
    }
}
