using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record DetallePrestamoReporteResponse
    {
        public int PrestamoId { get; set; }

        public int RecursoBibliograficoId { get; set; }

        public string TituloRecurso { get; set; } = string.Empty;

        public string IdentificadorEjemplar { get; set; } = string.Empty;

        public DateTime FechaPrestamo { get; set; }

        public DateTime FechaLimite { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}
