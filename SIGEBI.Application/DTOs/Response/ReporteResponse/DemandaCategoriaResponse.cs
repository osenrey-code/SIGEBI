
namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record DemandaCategoriaResponse
    {
        public string Categoria { get; init; } = string.Empty;

        public int CantidadSolicitada { get; init; }
    }
}
