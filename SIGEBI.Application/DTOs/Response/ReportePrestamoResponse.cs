

namespace SIGEBI.Application.DTOs.Response
{
    public class ReportePrestamoResponse
    {
        public int TotalPrestamos { get; set; }

        public int PrestamosSolicitados { get; set; }
        public int PrestamosActivos { get; set; }
        public int PrestamosDevueltos { get; set; }
        public int PrestamosRechazados { get; set; }
        public int PrestamosVencidos { get; set; }

        public int PrestamosDevueltosATiempo { get; set; }
        public int PrestamosDevueltosTarde { get; set; }

        public decimal TasaDevolucionPuntual { get; set; }
    }
}
