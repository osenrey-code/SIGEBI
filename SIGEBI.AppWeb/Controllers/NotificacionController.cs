using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.DTOs.Notificaciones;
using SIGEBI.AppWeb.Services;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class NotificacionController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<NotificacionController> _logger;

        public NotificacionController(IApiClient apiClient, ILogger<NotificacionController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool? soloNoLeidas)
        {
            try
            {
                bool esSoloNoLeidas = soloNoLeidas ?? false;
                string endpoint = $"api/notificacion/consultar?soloNoLeidas={esSoloNoLeidas.ToString().ToLower()}";

                var notificaciones = await _apiClient.GetAsync<IEnumerable<NotificacionResponse>>(endpoint);
                var lista = notificaciones?.ToList() ?? new List<NotificacionResponse>();

                ViewBag.SoloNoLeidas = esSoloNoLeidas;
                return View(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las notificaciones desde la API.");
                TempData["Error"] = $"No se pudieron obtener las notificaciones: {ex.Message}";
                return View(new List<NotificacionResponse>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                // 1. Obtener el listado completo para extraer la notificación
                string endpointConsultar = "api/notificacion/consultar?soloNoLeidas=false";
                var notificaciones = await _apiClient.GetAsync<IEnumerable<NotificacionResponse>>(endpointConsultar);
                var notificacion = notificaciones?.FirstOrDefault(n => n.NotificacionId == id);

                if (notificacion == null)
                {
                    TempData["Error"] = $"La notificación #{id} no fue encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                // 2. Marcar como leída únicamente si estaba pendiente
                if (!notificacion.Leida)
                {
                    string endpointMarcar = $"api/notificacion/marcarleida/{id}";
                    await _apiClient.PostAsync(endpointMarcar, new { });
                    notificacion.Leida = true;
                }

                return View(notificacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar el detalle de la notificación {id}.");
                TempData["Error"] = $"Error de comunicación: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // 🟢 Alimenta la campanita trayendo ÚNICAMENTE las no leídas
        [HttpGet]
        public async Task<IActionResult> ObtenerMisNotificaciones()
        {
            try
            {
                string endpoint = "api/notificacion/consultar?soloNoLeidas=true";
                var notificaciones = await _apiClient.GetAsync<IEnumerable<NotificacionResponse>>(endpoint);

                return Json(notificaciones ?? new List<NotificacionResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las notificaciones no leídas desde la API.");
                return Json(new List<NotificacionResponse>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarcarLeida(int id)
        {
            try
            {
                string endpoint = $"api/notificacion/marcarleida/{id}";
                await _apiClient.PostAsync(endpoint, new { });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al marcar como leída la notificación {id}.");
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                string endpoint = $"api/notificacion/eliminar/{id}";
                await _apiClient.DeleteAsync(endpoint);
                TempData["Success"] = "Notificación eliminada correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la notificación {id}.");
                TempData["Error"] = "No se pudo eliminar la notificación.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}