namespace SIGEBI.AppWeb.Models.DTOs.Prestamos
{
    public class ConsultarPrestamosActivosRequest
    {
        public string? Identificacion { get; set; }
        public int? EjemplarId { get; set; }
        public int? RecursoBibliograficoId { get; set; }
    }
}
