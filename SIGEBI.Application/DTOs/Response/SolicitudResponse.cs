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


        // Propiedad opcional: Solo vendrá con datos si la solicitud fue rechazada
        public string? MotivoRechazo { get; init; }
    }
}
