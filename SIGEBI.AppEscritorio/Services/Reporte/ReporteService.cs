using SIGEBI.AppEscritorio.Dtos.Reporte;
using SIGEBI.AppEscritorio.Services.Api;
using SIGEBI.AppEscritorio.Services.Interfaces;
using SIGEBI.AppEscritorio.Services.Reporte;
using System.Threading.Tasks;

namespace SIGEBI.AppEscritorio.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IApiClient _apiClient;

        public ReporteService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ReporteInventarioResponseDto?> ObtenerReporteInventarioAsync()
        {
            return await _apiClient.GetTAsync<ReporteInventarioResponseDto>("api/reportes/inventario");
        }

        public async Task<byte[]?> DescargarInventarioPdfAsync()
        {
            return await _apiClient.GetByteArrayAsync("api/reportes/inventario/pdf");
        }

        public async Task<ReportePrestamoResponseDto?> ObtenerReportePrestamosAsync(ReporteRangoFRequestDto request)
        {
            string url = $"api/reportes/prestamos?FechaInicio={request.FechaInicio:yyyy-MM-dd}&FechaFin={request.FechaFin:yyyy-MM-dd}";
            return await _apiClient.GetTAsync<ReportePrestamoResponseDto>(url);
        }

        public async Task<byte[]?> DescargarPrestamosPdfAsync(ReporteRangoFRequestDto request)
        {
            string url = $"api/reportes/prestamos/pdf?FechaInicio={request.FechaInicio:yyyy-MM-dd}&FechaFin={request.FechaFin:yyyy-MM-dd}";
            return await _apiClient.GetByteArrayAsync(url);
        }

        public async Task<ReportePenalizacionesResponseDto?> ObtenerReportePenalizacionesAsync(ReporteRangoFRequestDto request)
        {
            string url = $"api/reportes/penalizaciones?FechaInicio={request.FechaInicio:yyyy-MM-dd}&FechaFin={request.FechaFin:yyyy-MM-dd}";
            return await _apiClient.GetTAsync<ReportePenalizacionesResponseDto>(url);
        }

        public async Task<byte[]?> DescargarPenalizacionesPdfAsync(ReporteRangoFRequestDto request)
        {
            string url = $"api/reportes/penalizaciones/pdf?FechaInicio={request.FechaInicio:yyyy-MM-dd}&FechaFin={request.FechaFin:yyyy-MM-dd}";
            return await _apiClient.GetByteArrayAsync(url);
        }

        public async Task<ReporteUsoCatalogoResponseDto?> ObtenerReporteUsoCatalogoAsync(ReporteRangoFRequestDto request)
        {
            string url = $"api/reportes/Usocatalogo?FechaInicio={request.FechaInicio:yyyy-MM-dd}&FechaFin={request.FechaFin:yyyy-MM-dd}";
            return await _apiClient.GetTAsync<ReporteUsoCatalogoResponseDto>(url);
        }
        public async Task<byte[]?> DescargarUsoCatalogoPdfAsync(ReporteRangoFRequestDto request)
        {
            string url = $"api/reportes/Usocatalogo/pdf?FechaInicio={request.FechaInicio:yyyy-MM-dd}&FechaFin={request.FechaFin:yyyy-MM-dd}";
            return await _apiClient.GetByteArrayAsync(url);
        }
    }
}