
namespace SIGEBI.Application.DTOs.Response
{
    public class PrestamoResponse
    {
        public Guid PrestamoId { get; set; }
        public Guid PerfilLectorId { get; set; }
        public Guid RecursoId { get; set; }

        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaLimite { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public string Estado { get; set; } = string.Empty;
        public string? MotivoRechazo { get; set; }

    }
}
