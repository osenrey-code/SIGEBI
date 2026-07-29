namespace SIGEBI.AppWeb.Models.ViewModels.Solicitudes
{
    public class SolicitudIndexViewModel
    {
        public string? Busqueda { get; set; }
        public List<SolicitudItemViewModel> Solicitudes { get; set; } = new();
    }

    public class SolicitudItemViewModel
    {
        public int SolicitudId { get; set; }
        public string TituloRecurso { get; set; } = string.Empty;
        public string IdentificadorEjemplar { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}