
namespace SIGEBI.AppEscritorio.Dtos.Prestamos
{
    public class ConsultarHistorialPrestamosRequest
    {
        public string? Identificacion { get; set; }
        public int? EjemplarId { get; set; }
        public int? RecursoBibliograficoId { get; set; }
    }
}
