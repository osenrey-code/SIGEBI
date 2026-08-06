using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Services;
using SIGEBI.AppWeb.Models.ViewModels.Solicitudes;
using SIGEBI.AppWeb.Models.DTOs.Prestamos;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Docente,Estudiante")]
    public class SolicitudesController : BaseController
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<SolicitudesController> _logger;

        public SolicitudesController(IApiClient apiClient, ILogger<SolicitudesController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Solicitar(int? ejemplarId, string? tituloLibro)
        {
            var modelo = new RegistrarSolicitudViewModel();

            if (ejemplarId.HasValue && ejemplarId.Value > 0)
            {
                modelo.EjemplarId = ejemplarId.Value;
                modelo.TituloLibro = tituloLibro ?? string.Empty;
            }


            return View(modelo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solicitar(RegistrarSolicitudViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var request = new RegistrarSolicitudRequest { EjemplarId = model.EjemplarId };
                await _apiClient.PostAsync("api/prestamos/solicitar", request);

                TempData["Success"] = "¡Solicitud de préstamo registrada exitosamente!.";
                return RedirectToAction("Index", "Catalogo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la solicitud.");
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }
    }
}