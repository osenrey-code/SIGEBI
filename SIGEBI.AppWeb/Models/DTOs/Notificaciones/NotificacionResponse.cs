namespace SIGEBI.AppWeb.Models.DTOs.Notificaciones
{
    public class NotificacionResponse
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public bool Leida { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}