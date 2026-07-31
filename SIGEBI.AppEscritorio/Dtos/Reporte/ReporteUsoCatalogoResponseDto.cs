
namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class ReporteUsoCatalogoResponseDto
    {
        public int TotalSolicitudes { get; set; }
        public decimal DisponibilidadPromedio { get; set; }
        public List<RecursoMasSolicitadoResponseDto> RecursosMasSolicitados { get; set; } = new();
        public List<DemandaCategoriaResponseDto> DemandaPorCategoria { get; set; } = new();
    }
}
