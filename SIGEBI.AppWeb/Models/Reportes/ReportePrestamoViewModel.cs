

namespace SIGEBI.AppWeb.Models.Reportes
{
    public class ReportePrestamoViewModel
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime FechaFin { get; set; } = DateTime.Now;
       
    }
}
