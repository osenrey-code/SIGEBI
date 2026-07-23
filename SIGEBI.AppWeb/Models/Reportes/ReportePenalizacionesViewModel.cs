using SIGEBI.Application.DTOs.Response.ReporteResponse;

namespace SIGEBI.AppWeb.Models.Reportes
{
    public class ReportePenalizacionesViewModel
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime FechaFin { get; set; } = DateTime.Now;
        public ReportePenalizacionesResponse Reporte { get; set; } = new();
    }
}
