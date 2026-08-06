
namespace SIGEBI.AppEscritorio.Dtos.Devoluciones
{
    public class DevolucionResponseDto
    {
        public int PrestamoId { get; set; }
        public string TituloRecurso { get; set; } = string.Empty;
        public DateTime FechaDevolucion { get; set; }
        public int DiasRetraso { get; set; }
        public string Condicion { get; set; } = string.Empty;
        public bool PenalizacionGenerada { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string IdentificacionUsuario { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
    }
}
