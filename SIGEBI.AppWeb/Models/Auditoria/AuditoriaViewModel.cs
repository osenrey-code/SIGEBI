namespace SIGEBI.AppWeb.Models.Auditoria
{
    public class AuditoriaViewModel
    {
        public int AuditoriaId { get; set; }
        public int UsuarioId { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string EntidadAfectada { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}