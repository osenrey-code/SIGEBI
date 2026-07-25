using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.AppWeb.Models.Penalizaciones
{
    public class PenalizacionFiltroViewModel
    {
        public int? UsuarioId { get; set; }
        public int? PrestamoId { get; set; }
        public string? Estado { get; set; }

        public List<PenalizacionResponse> Penalizaciones { get; set; } = new();
    }
}