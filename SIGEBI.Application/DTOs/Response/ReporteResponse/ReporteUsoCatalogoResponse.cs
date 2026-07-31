

namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record ReporteUsoCatalogoResponse
    {
        public int TotalSolicitudes { get; init; }

        public decimal DisponibilidadPromedio { get; init; }

        public List<RecursoMasSolicitadoResponse> RecursosMasSolicitados { get; init; } = [];

        public List<DemandaCategoriaResponse> DemandaPorCategoria { get; init; } = [];
    }
}
