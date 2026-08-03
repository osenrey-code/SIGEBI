using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    [Authorize]
    public class ReporteController : BaseApiController
    {
        private readonly IGestionReportesUseCase _reportes;

        public ReporteController(IGestionReportesUseCase reportes)
        {
            _reportes = reportes;
        }

        [HttpGet("inventario")]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
        public async Task<IActionResult> ReporteInventario()
        {
            int usuarioId = ObtenerUsuarioId();
            var inventario = await _reportes.GenerarReporteInventarioAsync(usuarioId);
            return Ok(inventario);
        }

        [HttpGet("prestamos")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> ReportePrestamos([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            var prestamos = await _reportes.GenerarReportePrestamosAsync(request, usuarioId);
            return Ok(prestamos);
        }

        [HttpGet("penalizaciones")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> ReportePenalizaciones([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            var penalizaciones = await _reportes.GenerarReportePenalizacionesAsync(request, usuarioId);
            return Ok(penalizaciones);
        }

        [HttpGet("Usocatalogo")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> ReporteCatalogo([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            var catalogo = await _reportes.GenerarReporteUsoCatalogoAsync(request, usuarioId);
            return Ok(catalogo);
        }

        [HttpGet("inventario/pdf")]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
        public async Task<IActionResult> GenerarInventarioPdf()
        {
            int usuarioId = ObtenerUsuarioId();
            byte[] pdf = await _reportes.GenerarReporteInventarioPdfAsync(usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_Inventario.pdf");
        }

        [HttpGet("prestamos/pdf")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> GenerarPrestamosPdf([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            byte[] pdf = await _reportes.GenerarReportePrestamosPdfAsync(request, usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_Prestamos.pdf");
        }

        [HttpGet("penalizaciones/pdf")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> GenerarPenalizacionPdf([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            byte[] pdf = await _reportes.GenerarReportePenalizacionesPdfAsync(request, usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_Penalizaciones.pdf");
        }

        [HttpGet("Usocatalogo/pdf")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> GenerarCatalogoPdf([FromQuery] ReporteRangoFRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            byte[] pdf = await _reportes.GenerarReporteUsoCatalogoPdfAsync(request, usuarioId);
            return File(pdf, "application/pdf", "Reporte_SIGEBI_CatalogoUso.pdf");
        }
    }
}
