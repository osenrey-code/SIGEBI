
using SIGEBI.AppEscritorio.Dtos.Reporte;

namespace SIGEBI.AppEscritorio.Services.Reporte
{
    public interface IReporteService
    {
        Task<ReporteInventarioResponseDto?> ObtenerReporteInventarioAsync();
        Task<byte[]?> DescargarInventarioPdfAsync();

        Task<ReportePrestamoResponseDto?> ObtenerReportePrestamosAsync(ReporteRangoFRequestDto request);
        Task<byte[]?> DescargarPrestamosPdfAsync(ReporteRangoFRequestDto request);

        Task<ReportePenalizacionesResponseDto?> ObtenerReportePenalizacionesAsync(ReporteRangoFRequestDto request);
        Task<byte[]?> DescargarPenalizacionesPdfAsync(ReporteRangoFRequestDto request);

        Task<ReporteUsoCatalogoResponseDto?> ObtenerReporteUsoCatalogoAsync(ReporteRangoFRequestDto request);
        Task<byte[]?> DescargarUsoCatalogoPdfAsync(ReporteRangoFRequestDto request);
    }
}
