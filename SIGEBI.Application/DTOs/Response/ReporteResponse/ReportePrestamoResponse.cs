using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record ReportePrestamoResponse
    {
        public int TotalPrestamos { get; set; }

        public int PrestamosDevueltosATiempo { get; set; }

        public int PrestamosVencidos { get; set; }

        public decimal TasaDevolucionPuntual { get; set; }

        public List<DetallePrestamoReporteResponse> Prestamos { get; set; } = [];
    }
}
