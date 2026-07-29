using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.Catalogo;
using SIGEBI.AppWeb.Models.DTOs;
using SIGEBI.AppWeb.Services;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class CatalogoController : Controller
    {
        private readonly ApiClient _apiClient;
        private readonly ILogger<CatalogoController> _logger;

        public CatalogoController(ApiClient apiClient, ILogger<CatalogoController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? busqueda)
        {
            var viewModel = new CatalogoViewModel { Busqueda = busqueda };

            try
            {
                string endpoint = string.IsNullOrWhiteSpace(busqueda)
                    ? "api/recursos"
                    : $"api/recursos?busqueda={Uri.EscapeDataString(busqueda)}";

                // Petición HTTP a la API usando _apiClient
                var recursos = await _apiClient.GetAsync<IEnumerable<RecursoResponse>>(endpoint);
                viewModel.Recursos = recursos ?? new List<RecursoResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el catálogo vía API.");
                TempData["Error"] = ex.Message; // Captura del middleware de la API
                viewModel.Recursos = new List<RecursoResponse>();
            }

            return View(viewModel);
        }
    }
}