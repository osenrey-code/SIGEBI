using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.DTOs;
using SIGEBI.AppWeb.Models.Penalizaciones;
using SIGEBI.AppWeb.Services;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class PenalizacionesController : Controller
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<PenalizacionesController> _logger;

        public PenalizacionesController(ApiClient apiClient, ILogger<PenalizacionesController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new MisPenalizacionesViewModel();

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new Exception("No se pudo identificar la sesión del usuario actual.");
                }

                string endpoint = $"api/penalizaciones/usuario/{userIdClaim}";
                var misPenalizaciones = await _apiClient.GetAsync<IEnumerable<PenalizacionResponse>>(endpoint);

                if (misPenalizaciones != null)
                {
                    viewModel.Activas = misPenalizaciones
                        .Where(p => p.EstaActiva)
                        .OrderByDescending(p => p.FechaInicio)
                        .ToList();

                    viewModel.Historial = misPenalizaciones
                        .Where(p => !p.EstaActiva)
                        .OrderByDescending(p => p.FechaInicio)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el historial de penalizaciones.");
                TempData["Error"] = ex.Message;
            }

            return View(viewModel);
        }
    }
}