

namespace SIGEBI.AppWeb.Models.Reportes
{
    public class ReporteUsoCatalogoViewModel
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime FechaFin { get; set; } = DateTime.Now;
       
    }
}

