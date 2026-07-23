using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Auditor,Bibliotecario")]
    public class ReportesController : BaseController
    {
        private readonly IGestionReportesUseCase _reportes;

        public ReportesController(IGestionReportesUseCase reportes)
        {
            _reportes = reportes;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DescargarInventarioPdf()
        {
            try
            {
                int usuarioId = ObtenerUsuarioId();
                byte[] pdfBytes = await _reportes.GenerarReporteInventarioPdfAsync(usuarioId);
                return File(pdfBytes, "application/pdf", "Reporte_Inventario_SIGEBI.pdf");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DescargarPrestamosPdf(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var request = new ReporteRangoFRequest
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                };
                int usuarioId = ObtenerUsuarioId();
                byte[] pdfBytes = await _reportes.GenerarReportePrestamosPdfAsync(request, usuarioId);
                return File(pdfBytes, "application/pdf", "Reporte_Prestamos_SIGEBI.pdf");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DescargarPenalizacionesPdf(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var request = new ReporteRangoFRequest
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                };
                int usuarioId = ObtenerUsuarioId();
                byte[] pdfBytes = await _reportes.GenerarReportePenalizacionesPdfAsync(request, usuarioId);
                return File(pdfBytes, "application/pdf", "Reporte_Penalizaciones_SIGEBI.pdf");
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}