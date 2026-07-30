using SIGEBI.AppWeb.Models.DTOs.Penalizaciones;

namespace SIGEBI.AppWeb.Models.ViewModels.Penalizaciones
{
    public class PenalizacionesActivasViewModel
    {
        public IEnumerable<PenalizacionResponse> Penalizaciones { get; set; } = new List<PenalizacionResponse>();
    }
}