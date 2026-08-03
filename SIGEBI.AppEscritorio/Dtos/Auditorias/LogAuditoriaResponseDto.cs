
namespace SIGEBI.AppEscritorio.Dtos.Auditorias
{
    public class LogAuditoriaResponseDto
    {
        public int AuditoriaId { get; set; }
        public int UsuarioId { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string EntidadAfectada { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
