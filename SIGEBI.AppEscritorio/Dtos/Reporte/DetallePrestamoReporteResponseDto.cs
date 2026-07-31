
namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class DetallePrestamoReporteResponseDto
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
