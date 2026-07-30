using SIGEBI.AppWeb.Models.DTOs;

namespace SIGEBI.AppWeb.Models.Penalizaciones
{
    public class MisPenalizacionesViewModel
    {
        public IEnumerable<PenalizacionResponse> Activas { get; set; } = new List<PenalizacionResponse>();
        public IEnumerable<PenalizacionResponse> Historial { get; set; } = new List<PenalizacionResponse>();
    }
}