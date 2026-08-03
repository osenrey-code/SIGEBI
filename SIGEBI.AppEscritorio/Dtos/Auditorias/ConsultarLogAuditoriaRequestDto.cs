
namespace SIGEBI.AppEscritorio.Dtos.Auditorias
{
    public class ConsultarLogAuditoriaRequestDto
    {
        public int? UsuarioId { get; set; }
        public string? Accion { get; set; }
        public string? EntidadAfectada { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
