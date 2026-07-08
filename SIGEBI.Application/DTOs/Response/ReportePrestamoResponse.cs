

namespace SIGEBI.Application.DTOs.Response
{
    public record ReportePrestamoResponse
    {
        public int TotalPrestamos { get; init; }
        public int DevolucionesPuntuales { get; init; }
        public int PrestamosVencidos { get; init; }
        public decimal TasaDevolucionPuntual { get; init; }
    }
}
