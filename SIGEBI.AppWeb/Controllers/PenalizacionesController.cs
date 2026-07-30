using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.DTOs.Penalizaciones;
using SIGEBI.AppWeb.Models.ViewModels.Penalizaciones;
using SIGEBI.AppWeb.Services;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Docente,Estudiante")]
    public class PenalizacionesController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<PenalizacionesController> _logger;

        public PenalizacionesController(IApiClient apiClient, ILogger<PenalizacionesController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new PenalizacionesActivasViewModel();

            try
            {
                // 1. Tomamos el ID del usuario de la sesión, sin importar su rol
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new Exception("No se pudo identificar la sesión del usuario actual.");
                }

                // 2. Apuntamos al endpoint de activas y le pasamos el ID directamente
                string endpoint = $"api/penalizaciones/consultar/activas?UsuarioId={userIdClaim}";

                var resultado = await _apiClient.GetAsync<IEnumerable<PenalizacionResponse>>(endpoint);

                if (resultado != null)
                {
                    viewModel.Penalizaciones = resultado;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar las penalizaciones activas.");
                TempData["Error"] = "No se pudieron cargar las penalizaciones en este momento.";
            }

            return View(viewModel);
        }

    }
}