
namespace SIGEBI.Application.DTOs.Request
{
    public record ConsultarHistorialDevolucionesRequest
    {
        public int? UsuarioId { get; init; }
        public int? RecursoBibliograficoId { get; init; }
        public int? EjemplarId { get; init; }
        public DateTime? FechaInicio { get; init; }
        public DateTime? FechaFin { get; init; }
    }
}
