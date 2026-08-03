
namespace SIGEBI.AppEscritorio.Dtos.Devoluciones
{
    public class ConsultarHistorialDevolucionesRequestDto
    {
        public int? UsuarioId { get; set; }
        public int? RecursoBibliograficoId { get; set; }
        public int? EjemplarId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
