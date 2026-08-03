
namespace SIGEBI.AppEscritorio.Dtos.Penalizaciones
{
    public class ResolverPenalizacionRequestDto
    {
        public int PenalizacionId { get; set; }
        public string MotivoResolucion { get; set; } = string.Empty;
    }
}
