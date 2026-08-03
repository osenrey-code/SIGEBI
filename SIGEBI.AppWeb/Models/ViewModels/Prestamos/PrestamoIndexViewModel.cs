namespace SIGEBI.AppWeb.Models.ViewModels.Prestamos
{
    public class PrestamoIndexViewModel
    {
        public List<PrestamoItemViewModel> Prestamos { get; set; } = new();
    }

    public class PrestamoItemViewModel
    {
        public int PrestamoId { get; set; }
        public string TituloRecurso { get; set; } = string.Empty;
        public string IdentificadorEjemplar { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; } = string.Empty;
        public bool EstaVencido { get; set; }
    }

}
