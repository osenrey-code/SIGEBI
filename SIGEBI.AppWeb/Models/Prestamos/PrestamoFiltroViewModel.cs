namespace SIGEBI.AppWeb.Models.Prestamos
{
    public class PrestamoFiltroViewModel
    {
        public string? Identificacion { get; set; }
        public int? RecursoBibliograficoId { get; set; }
        public int? EjemplarId { get; set; }
        public List<PrestamoItemViewModel> Prestamos { get; set; } = new();
    }
}