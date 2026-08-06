

namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record RecursoMasSolicitadoResponse
    {
        public int RecursoBibliograficoId { get; init; }
        public string Titulo { get; init; } = string.Empty;
        public int CantidadSolicitudes { get; init; }
        public string Autor { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
    }
}
