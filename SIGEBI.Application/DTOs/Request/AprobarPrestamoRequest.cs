

namespace SIGEBI.Application.DTOs.Request
{
    public class AprobarPrestamoRequest
    {
        public Guid PrestamoId { get; set; }
        public Guid EjecutorId { get; set; }
        public int DiasPermitidos { get; set; }
    }
}
