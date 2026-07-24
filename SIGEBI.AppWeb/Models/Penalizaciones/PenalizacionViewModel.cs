namespace SIGEBI.AppWeb.Models.Penalizaciones
{
    public class PenalizacionViewModel
    {
        public int PenalizacionId { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime FechaPenalizacion { get; set; }
        public bool Resuelta { get; set; }
    }
}