
namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class ReportePrestamoResponseDto
    {
        public int TotalPrestamos { get; set; }
        public int PrestamosDevueltosATiempo { get; set; }
        public int PrestamosVencidos { get; set; }
        public decimal TasaDevolucionPuntual { get; set; }
        public List<DetallePrestamoReporteResponseDto> Prestamos { get; set; } = new();
    }
}
