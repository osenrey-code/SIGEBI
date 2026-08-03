
namespace SIGEBI.Application.DTOs.Response
{
    public record PrestamoResponse
    {
        public int PrestamoId { get; init; }
        public string TituloRecurso { get; init; } = string.Empty;
        public string IdentificadorEjemplar { get; init; } = string.Empty;

        public DateTime FechaInicio { get; init; }
        public DateTime FechaLimite { get; init; }

        public string Estado { get; init; } = string.Empty;
        public bool EstaVencido => DateTime.UtcNow.Date > FechaLimite.Date;
    }
}
