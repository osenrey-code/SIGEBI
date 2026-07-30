//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SIGEBI.Application.DTOs.Request;
//using SIGEBI.Application.Interfaces.Service;
//using SIGEBI.AppWeb.Models.Solicitudes;
//using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class SolicitudesController : BaseController
    {
        private readonly IGestionPrestamos _gestionPrestamos;
        private readonly IGestionCatalogo _gestionCatalogo;
        private readonly ILogger<SolicitudesController> _logger;

        public SolicitudesController(
            IGestionPrestamos gestionPrestamos,
            IGestionCatalogo gestionCatalogo,
            ILogger<SolicitudesController> logger)
        {
            _gestionPrestamos = gestionPrestamos;
            _gestionCatalogo = gestionCatalogo;
            _logger = logger;
        }

    
//        [HttpGet]
//        public IActionResult Solicitar(int? ejemplarId)
//        {
//            var modelo = new RegistrarSolicitudViewModel();

//            if (ejemplarId.HasValue && ejemplarId.Value > 0)
//            {
//                modelo.EjemplarId = ejemplarId.Value;
//            }

         
//            return View(modelo);
//        }


        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var detalle = await _gestionPrestamos.ObtenerPorIdConDetallesAsync(id);
                if (detalle == null)
                {
                    TempData["Error"] = "La solicitud especificada no existe.";
                    return RedirectToAction(nameof(Index));
                }
                return View(detalle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles de la solicitud {Id}", id);
                TempData["Error"] = "Ocurrió un error al cargar los detalles.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Docente,Estudiante")]
        [HttpGet]
        public async Task<IActionResult> Solicitar(int? recursoId, int? ejemplarId)
        {
            var modelo = new RegistrarSolicitudViewModel();

            // 1. Si ya viene el ejemplarId específico directamente
            if (ejemplarId.HasValue && ejemplarId.Value > 0)
            {
                modelo.EjemplarId = ejemplarId.Value;
                return View(modelo);
            }

            // 2. Si viene el recursoId (ej. el libro 8), consultamos el ID de su primer ejemplar disponible de forma limpia
            if (recursoId.HasValue && recursoId.Value > 0)
            {
                try
                {
                    var idEjemplarDisponible = await _gestionCatalogo.ObtenerPrimerEjemplarDisponibleIdAsync(recursoId.Value);

                    if (idEjemplarDisponible.HasValue && idEjemplarDisponible.Value > 0)
                    {
                        modelo.EjemplarId = idEjemplarDisponible.Value; // Inyecta limpiamente el EjemplarId real (ej. 31)
                    }
                    else
                    {
                        TempData["Error"] = "No hay ejemplares disponibles para este recurso en este momento.";
                        return RedirectToAction("Index", "Catalogo");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener el ejemplar disponible para el recurso {RecursoId}", recursoId);
                    TempData["Error"] = "Ocurrió un error al procesar la solicitud.";
                    return RedirectToAction("Index", "Catalogo");
                }
            }

            return View(modelo);
        }

        [Authorize(Roles = "Docente,Estudiante")]
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

//                TempData["Success"] = "Solicitud de préstamo registrada correctamente.";

//                // CORREGIDO: Redirigir al Catálogo (Index) o Préstamos Activos en vez de Solicitudes/Index 
//                // (ya que Solicitudes/Index es exclusivo de Bibliotecario/Admin y daría error 403 a Docente/Estudiante).
//                return RedirectToAction("Index", "Catalogo");
//            }
//            catch (BusinessException ex)
//            {
//                ModelState.AddModelError(string.Empty, ex.Message);
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al registrar la solicitud.");
//                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
//                return View(model);
//            }
//        }
//    }
//}