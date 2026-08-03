
namespace SIGEBI.Application.DTOs.Request
{
    public record ConsultarPrestamosActivosRequest
    {
        public string? Identificacion { get; init; }
        public int? EjemplarId { get; init; }
        public int? RecursoBibliograficoId { get; init; }
    }
}
