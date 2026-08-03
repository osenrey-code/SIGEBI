
namespace SIGEBI.AppEscritorio.Dtos.Penalizaciones
{
    public class ConsultarPenalizacionesRequestDto
    {
        public int? UsuarioId { get; set; }
        public int? PrestamoId { get; set; }
        public string? Estado { get; set; }
    }
}
