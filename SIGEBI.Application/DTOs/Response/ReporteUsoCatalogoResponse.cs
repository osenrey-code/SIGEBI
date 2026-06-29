
namespace SIGEBI.Application.DTOs.Response
{
    public class ReporteUsoCatalogoResponse
    {
        public int TotalSolicitudes { get; set; }
        public int RecursosDiferentesSolicitados { get; set; }
        public decimal PorcentajeDisponibilidadActual { get; set; }

        public List<RecursoSolicitadoReporteResponse> RecursosMasSolicitados { get; set; } = new();
        public List<DemandaCategoriaReporteResponse> DemandaPorCategoria { get; set; } = new();
    }

    public class RecursoSolicitadoReporteResponse
    {
        public Guid RecursoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadSolicitudes { get; set; }
    }

    public class DemandaCategoriaReporteResponse
    {
        public string Categoria { get; set; } = string.Empty;
        public int CantidadSolicitudes { get; set; }
    }


}
