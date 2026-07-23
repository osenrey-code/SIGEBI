using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Reportes;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Auditor,Bibliotecario")]
    public class ReportesController : BaseController
    {
        private readonly IGestionReportesUseCase _gestionReportes;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(IGestionReportesUseCase gestionReportes, ILogger<ReportesController> logger)
        {
            _gestionReportes = gestionReportes;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


      
        [HttpGet]
        public async Task<IActionResult> Inventario()
        {
            try
            {
                var resultado = await _gestionReportes.GenerarReporteInventarioAsync(ObtenerUsuarioId());
                return View(resultado);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de inventario.");
                TempData["Error"] = "Ocurrió un error al cargar el reporte de inventario.";
                return RedirectToAction(nameof(Index));
            }
        }

       
        [HttpGet]
        public async Task<IActionResult> InventarioPdf()
        {
            try
            {
                var pdfBytes = await _gestionReportes.GenerarReporteInventarioPdfAsync(ObtenerUsuarioId());
                return File(pdfBytes, "application/pdf", $"Reporte_Inventario_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte de inventario en PDF.");
                TempData["Error"] = "No se pudo generar el PDF del reporte de inventario.";
                return RedirectToAction(nameof(Inventario));
            }
        }

        [Authorize(Roles = "Administrador,Auditor")]
        [HttpGet]
        public async Task<IActionResult> Prestamos(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var modelo = new ReportePrestamoViewModel
            {
                FechaInicio = fechaInicio ?? DateTime.Now.AddMonths(-1),
                FechaFin = fechaFin ?? DateTime.Now
            };

            try
            {
                var request = new ReporteRangoFRequest
                {
                    FechaInicio = modelo.FechaInicio,
                    FechaFin = modelo.FechaFin
                };

                modelo.Reporte = await _gestionReportes.GenerarReportePrestamosAsync(request, ObtenerUsuarioId());
                return View(modelo);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de préstamos.");
                TempData["Error"] = "Ocurrió un error al cargar el reporte de préstamos.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador,Auditor")]
        [HttpGet]
        public async Task<IActionResult> PrestamosPdf(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var request = new ReporteRangoFRequest { FechaInicio = fechaInicio, FechaFin = fechaFin };
                var pdfBytes = await _gestionReportes.GenerarReportePrestamosPdfAsync(request, ObtenerUsuarioId());
                return File(pdfBytes, "application/pdf", $"Reporte_Prestamos_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte de préstamos en PDF.");
                TempData["Error"] = "No se pudo generar el PDF del reporte de préstamos.";
                return RedirectToAction(nameof(Prestamos), new { fechaInicio, fechaFin });
            }
        }

        [Authorize(Roles = "Administrador,Auditor")]
        [HttpGet]
        public async Task<IActionResult> Penalizaciones(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var modelo = new ReportePenalizacionesViewModel
            {
                FechaInicio = fechaInicio ?? DateTime.Now.AddMonths(-1),
                FechaFin = fechaFin ?? DateTime.Now
            };

            try
            {
                var request = new ReporteRangoFRequest
                {
                    FechaInicio = modelo.FechaInicio,
                    FechaFin = modelo.FechaFin
                };

                modelo.Reporte = await _gestionReportes.GenerarReportePenalizacionesAsync(request, ObtenerUsuarioId());
                return View(modelo);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de penalizaciones.");
                TempData["Error"] = "Ocurrió un error al cargar el reporte de penalizaciones.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador,Auditor")]
        [HttpGet]
        public async Task<IActionResult> PenalizacionesPdf(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var request = new ReporteRangoFRequest { FechaInicio = fechaInicio, FechaFin = fechaFin };
                var pdfBytes = await _gestionReportes.GenerarReportePenalizacionesPdfAsync(request, ObtenerUsuarioId());
                return File(pdfBytes, "application/pdf", $"Reporte_Penalizaciones_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte de penalizaciones en PDF.");
                TempData["Error"] = "No se pudo generar el PDF del reporte de penalizaciones.";
                return RedirectToAction(nameof(Penalizaciones), new { fechaInicio, fechaFin });
            }
        }

        [Authorize(Roles = "Administrador,Auditor")]
        [HttpGet]
        public async Task<IActionResult> UsoCatalogo(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var modelo = new ReporteUsoCatalogoViewModel
            {
                FechaInicio = fechaInicio ?? DateTime.Now.AddMonths(-1),
                FechaFin = fechaFin ?? DateTime.Now
            };

            try
            {
                var request = new ReporteRangoFRequest
                {
                    FechaInicio = modelo.FechaInicio,
                    FechaFin = modelo.FechaFin
                };

                modelo.Reporte = await _gestionReportes.GenerarReporteUsoCatalogoAsync(request, ObtenerUsuarioId());
                return View(modelo);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de uso de catálogo.");
                TempData["Error"] = "Ocurrió un error al cargar el reporte de uso de catálogo.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador,Auditor")]
        [HttpGet]
        public async Task<IActionResult> UsoCatalogoPdf(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var request = new ReporteRangoFRequest { FechaInicio = fechaInicio, FechaFin = fechaFin };
                var pdfBytes = await _gestionReportes.GenerarReporteUsoCatalogoPdfAsync(request, ObtenerUsuarioId());
                return File(pdfBytes, "application/pdf", $"Reporte_UsoCatalogo_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte de uso de catálogo en PDF.");
                TempData["Error"] = "No se pudo generar el PDF del reporte de uso de catálogo.";
                return RedirectToAction(nameof(UsoCatalogo), new { fechaInicio, fechaFin });
            }
        }
    }
}