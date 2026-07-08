
namespace SIGEBI.Application.DTOs.Response
{
    public record ReporteUsoCatalogoResponse
    {
        public string Categoria { get; init; } = string.Empty;
        public int TotalPrestamos { get; init; }
        public string RecursoMasSolicitado { get; init; } = string.Empty;
        public decimal DisponibilidadPromedio { get; init; }
    }
}
