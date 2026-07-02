
namespace SIGEBI.Application.DTOs.Response
{
    public class PrestamoResponse
    {
        public int PrestamoId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; } = string.Empty;

    }
}
