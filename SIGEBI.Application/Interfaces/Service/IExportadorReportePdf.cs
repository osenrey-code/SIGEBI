using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response.ReporteResponse;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IExportadorReportePdf
    {
        byte[] GenerarReportePrestamosPdf(
            ReportePrestamoResponse reporte,
            ReporteRangoFRequest rango
        );

        byte[] GenerarReportePenalizacionesPdf(
            ReportePenalizacionesResponse reporte,
            ReporteRangoFRequest rango
        );

        byte[] GenerarReporteUsoCatalogoPdf(
            ReporteUsoCatalogoResponse reporte,
            ReporteRangoFRequest rango
        );

        byte[] GenerarReporteInventarioPdf(
            ReporteInventarioResponse reporte
        );
    }
}