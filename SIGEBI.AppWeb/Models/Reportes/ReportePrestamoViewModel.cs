using SIGEBI.Application.DTOs.Response.ReporteResponse;

namespace SIGEBI.AppWeb.Models.Reportes
{
    public class ReportePrestamoViewModel
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime FechaFin { get; set; } = DateTime.Now;
        public ReportePrestamoResponse Reporte { get; set; } = new();
    }
}
