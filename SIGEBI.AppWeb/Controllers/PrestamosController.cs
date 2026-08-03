using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Services;
using SIGEBI.AppWeb.Models.DTOs.Prestamos;
using SIGEBI.AppWeb.Models.ViewModels.Solicitudes;
using SIGEBI.AppWeb.Models.ViewModels.Prestamos;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Docente,Estudiante")]
    public class PrestamosController : BaseController
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<PrestamosController> _logger;

        public PrestamosController(IApiClient apiClient, ILogger<PrestamosController> logger)
        {
            _apiClient = apiClient; 
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Activos));
        }

        [HttpGet]
        public async Task<IActionResult> Activos()
        {
            var modelo = new PrestamoIndexViewModel();

            try
            {
                var respuesta = await _apiClient.GetAsync<List<PrestamoDto>>("api/prestamos/consultar/activos")
                    ?? new List<PrestamoDto>();

                modelo.Prestamos = respuesta.Select(p => new PrestamoItemViewModel
                {
                    PrestamoId = p.PrestamoId,
                    TituloRecurso = p.TituloRecurso,
                    IdentificadorEjemplar = p.IdentificadorEjemplar,
                    FechaInicio = p.FechaInicio,
                    FechaLimite = p.FechaLimite,
                    Estado = p.Estado,
                    EstaVencido = p.EstaVencido
                }).ToList();

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Errpr al consultar los préstamos activos.");
                TempData["Error"] = ex.Message;
            }

            return View("Index", modelo);
        }
    }
}