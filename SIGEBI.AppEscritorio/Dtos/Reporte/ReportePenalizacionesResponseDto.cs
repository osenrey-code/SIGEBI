
namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class ReportePenalizacionesResponseDto
    {
        public int TotalPenalizaciones { get; set; }
        public int PenalizacionesActivas { get; set; }
        public int PenalizacionesResueltas { get; set; }
        public int TotalDiasRetraso { get; set; }
        public decimal MontoTotalMora { get; set; }
        public decimal MontoMoraActiva { get; set; }
        public decimal MontoMoraResuelta { get; set; }
        public List<PenalizacionPorTipoUsuarioResponseDto> PorTipoUsuario { get; set; } = new();
        public List<DetallePenalizacionReporteResponseDto> Detalles { get; set; } = new();
    }
}
