using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Penalizaciones;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class PenalizacionesController : BaseController
    {
        private readonly IGestionPenalizaciones _gestionPenalizaciones;
        private readonly ILogger<PenalizacionesController> _logger;

        public PenalizacionesController(
            IGestionPenalizaciones gestionPenalizaciones,
            ILogger<PenalizacionesController> logger)
        {
            _gestionPenalizaciones = gestionPenalizaciones;
            _logger = logger;
        }

        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
        [HttpGet]
        public async Task<IActionResult> Index(int? usuarioId, int? prestamoId, string? estado)
        {
            try
            {
                var esLector = User.IsInRole("Estudiante") || User.IsInRole("Docente");

                // Si es estudiante o docente, forzamos la consulta a su propio UsuarioId
                var usuarioIdFiltro = esLector ? ObtenerUsuarioId() : usuarioId;

                var request = new ConsultarPenalizacionesRequest
                {
                    UsuarioId = usuarioIdFiltro,
                    PrestamoId = prestamoId,
                    Estado = estado
                };

                var respuesta = await _gestionPenalizaciones.ConsultarPenalizacionesAsync(request, ObtenerUsuarioId());

                var modelo = new PenalizacionFiltroViewModel
                {
                    UsuarioId = usuarioIdFiltro,
                    PrestamoId = prestamoId,
                    Estado = estado,
                    Penalizaciones = respuesta.ToList()
                };

                return View(modelo);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new PenalizacionFiltroViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las penalizaciones.");
                TempData["Error"] = "No se pudieron cargar las penalizaciones.";
                return View(new PenalizacionFiltroViewModel());
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolver(int penalizacionId, string motivo)
        {
            try
            {
                var request = new ResolverPenalizacionRequest
                {
                    PenalizacionId = penalizacionId,
                    MotivoResolucion = motivo
                };

                await _gestionPenalizaciones.ResolverPenalizacionAsync(request, ObtenerUsuarioId());

                TempData["Success"] = $"La penalización #{penalizacionId} ha sido resuelta exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al resolver la penalización {Id}", penalizacionId);
                TempData["Error"] = "Ocurrió un error al procesar la resolución de la penalización.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}