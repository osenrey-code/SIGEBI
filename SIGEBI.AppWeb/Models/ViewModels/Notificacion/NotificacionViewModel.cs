namespace SIGEBI.AppWeb.Models.ViewModels.Notificacion
{
    public class NotificacionViewModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; 
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Leida { get; set; }
        public string? TargetUrl { get; set; } 
    }
}
