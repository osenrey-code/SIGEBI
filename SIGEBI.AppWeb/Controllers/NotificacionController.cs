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

        // Este método alimenta la campanita de notificaciones
        [HttpGet]
        public async Task<IActionResult> ObtenerMisNotificaciones()
        {
            try
            {
                // Apuntamos al endpoint de tu API. 
                // Como la API usa [Route("api/[controller]")] y [HttpGet("consultar")]
                // la ruta final es api/notificacion/consultar
                string endpoint = "api/notificacion/consultar";

                var notificaciones = await _apiClient.GetAsync<IEnumerable<NotificacionResponse>>(endpoint);

                // Devolvemos el JSON para que el script del frontend lo renderice
                return Json(notificaciones ?? new List<NotificacionResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las notificaciones desde la API.");
                // Si hay error, devolvemos una lista vacía para no romper el menú
                return Json(new List<NotificacionResponse>());
            }
        }

        // Este método se dispara cuando el usuario le da a "Marcar leída"
        [HttpPost]
        public async Task<IActionResult> MarcarLeida(int id)
        {
            try
            {
                // Apuntamos al endpoint de tu API para marcar como leída
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

    }
}