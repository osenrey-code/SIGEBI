namespace SIGEBI.AppWeb.Models.DTOs
{
    public class PenalizacionResponse
    {
        public int PenalizacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public bool EstaActiva { get; set; }
    }
}