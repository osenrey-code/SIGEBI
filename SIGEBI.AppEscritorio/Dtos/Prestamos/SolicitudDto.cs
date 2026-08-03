using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.AppEscritorio.Dtos.Prestamos
{
    public class SolicitudDto
    {
        public int SolicitudId { get; set; }
        public string TituloRecurso { get; set; } = string.Empty;
        public string IdentificadorEjemplar { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? MotivoRechazo { get; set; }
    }
}
