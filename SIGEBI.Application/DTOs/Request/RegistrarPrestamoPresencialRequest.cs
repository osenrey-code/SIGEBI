

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarPrestamoPresencialRequest
    {
        public Guid UsuarioId { get; set; }
        public Guid RecursoId { get; set; }
        public Guid EjecutorId { get; set; }
    }
}
