
namespace SIGEBI.Application.DTOs.Response
{
    public record DevolucionResponse
    {
        public int PrestamoId { get; init; }
        public string TituloRecurso { get; init; } = string.Empty;
        public DateTime FechaDevolucion { get; init; }
        public int DiasRetraso { get; init; }
        public string Condicion { get; init; } = string.Empty;
        public bool PenalizacionGenerada { get; init; }
        public decimal MontoPenalizacion { get; init; }
        public string Mensaje { get; init; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string IdentificacionUsuario { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
    }
}
