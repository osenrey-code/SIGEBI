using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response.ReporteResponse;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IGestionReportesUseCase
    {
        Task<ReporteInventarioResponse> GenerarReporteInventarioAsync(
            int usuarioEjecutorId
        );

        Task<byte[]> GenerarReporteInventarioPdfAsync(
            int usuarioEjecutorId
        );

        Task<ReportePrestamoResponse> GenerarReportePrestamosAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId
        );

        Task<byte[]> GenerarReportePrestamosPdfAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId
        );

        Task<ReportePenalizacionesResponse> GenerarReportePenalizacionesAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId
        );

        Task<byte[]> GenerarReportePenalizacionesPdfAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId
        );

        Task<ReporteUsoCatalogoResponse> GenerarReporteUsoCatalogoAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId
        );

        Task<byte[]> GenerarReporteUsoCatalogoPdfAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId
        );
    }
}