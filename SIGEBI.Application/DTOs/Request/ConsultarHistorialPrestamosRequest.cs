
namespace SIGEBI.Application.DTOs.Request
{
    public class ConsultarHistorialPrestamosRequest
    {
        public Guid? UsuarioId { get; set; }
        public Guid? RecursoId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
