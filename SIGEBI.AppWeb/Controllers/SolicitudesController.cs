using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Solicitudes;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Docente,Estudiante")]
    public class SolicitudesController : BaseController
    {
        private readonly IGestionPrestamos _gestionPrestamos;
        private readonly ILogger<SolicitudesController> _logger;

        public SolicitudesController(IGestionPrestamos gestionPrestamos, ILogger<SolicitudesController> logger)
        {
            _gestionPrestamos = gestionPrestamos;
            _logger = logger;
        }

    
        [HttpGet]
        public IActionResult Solicitar(int? ejemplarId)
        {
            var modelo = new RegistrarSolicitudViewModel();

            if (ejemplarId.HasValue && ejemplarId.Value > 0)
            {
                modelo.EjemplarId = ejemplarId.Value;
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
                await _gestionPrestamos.SolicitarPrestamoAsync(request, ObtenerUsuarioId());

                TempData["Success"] = "Solicitud de préstamo registrada correctamente.";

                // CORREGIDO: Redirigir al Catálogo (Index) o Préstamos Activos en vez de Solicitudes/Index 
                // (ya que Solicitudes/Index es exclusivo de Bibliotecario/Admin y daría error 403 a Docente/Estudiante).
                return RedirectToAction("Index", "Catalogo");
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la solicitud.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }
    }
}