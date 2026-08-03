
namespace SIGEBI.AppEscritorio.Dtos.Devoluciones
{
    public class RegistrarDevolucionRequestDto
    {
        public int PrestamoId { get; set; }
        public string Condicion { get; set; } = string.Empty;
        public string? Observacion { get; set; }
    }
}
