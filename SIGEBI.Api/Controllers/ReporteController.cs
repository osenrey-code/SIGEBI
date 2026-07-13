using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IGestionReportesUseCase _reportes;

        public ReporteController(IGestionReportesUseCase reportes)
        {
            _reportes = reportes;
        }

        [HttpGet("inventario")]
        public async Task<IActionResult> ReporteInventario()
        {
            int usuarioId = 1;
            var inventario = await _reportes.GenerarReporteInventarioAsync(usuarioId);
            return Ok(inventario);
        }

        [HttpGet("prestamos")]
        public async Task<IActionResult> ReportePrestamos([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = 1;
            var prestamos = await _reportes.GenerarReportePrestamosAsync(request, usuarioId);
            return Ok(prestamos);
        }

        [HttpGet("penalizaciones")]
        public async Task<IActionResult> ReportePenalizaciones([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = 1;
            var penalizaciones = await _reportes.GenerarReportePenalizacionesAsync(request, usuarioId);
            return Ok(penalizaciones);
        }

        [HttpGet("Usocatalogo")]
        public async Task<IActionResult> ReporteCatalogo([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = 1;
            var catalogo = await _reportes.GenerarReporteUsoCatalogoAsync(request, usuarioId);
            return Ok(catalogo);
        }

        [HttpGet("inventario/pdf")]
        public async Task<IActionResult> GenerarInventarioPdf()
        {
            int usuarioId = 1;
            byte[] pdf = await _reportes.GenerarReporteInventarioPdfAsync(usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_Inventario.pdf");
        }

        [HttpGet("prestamos/pdf")]
        public async Task<IActionResult> GenerarPrestamosPdf([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = 1;
            byte[] pdf = await _reportes.GenerarReportePrestamosPdfAsync(request, usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_Prestamos.pdf");
        }

        [HttpGet("penalizaciones/pdf")]
        public async Task<IActionResult> GenerarPenalizacionPdf([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = 1;
            byte[] pdf = await _reportes.GenerarReportePenalizacionesPdfAsync(request, usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_Penalizaciones.pdf");
        }

        [HttpGet("Usocatalogo/pdf")]
        public async Task<IActionResult> GenerarCatalogoPdf([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = 1;
            byte[] pdf = await _reportes.GenerarReporteUsoCatalogoPdfAsync(request, usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_CatalogoUso.pdf");
        }
    }
}
