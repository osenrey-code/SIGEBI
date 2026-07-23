using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.AppWeb.Models.Devoluciones
{
    public class HistorialDevolucionesViewModel
    {
        public int? UsuarioId { get; set; }
        public int? RecursoBibliograficoId { get; set; }
        public int? EjemplarId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public IEnumerable<DevolucionResponse> Devoluciones { get; set; } = new List<DevolucionResponse>();
    }
}
