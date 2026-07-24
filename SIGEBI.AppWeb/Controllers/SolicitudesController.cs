using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Solicitudes;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class SolicitudesController : BaseController
    {
        private readonly IGestionPrestamos _gestionPrestamos;
        private readonly ILogger<SolicitudesController> _logger;

        public SolicitudesController(IGestionPrestamos gestionPrestamos, ILogger<SolicitudesController> logger)
        {
            _gestionPrestamos = gestionPrestamos;
            _logger = logger;
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var respuesta = await _gestionPrestamos.ConsultarTodasAsync();

                var modelo = new SolicitudIndexViewModel
                {
                    Solicitudes = respuesta.Select(s => new SolicitudItemViewModel
                    {
                        SolicitudId = s.SolicitudId,
                        TituloRecurso = s.TituloRecurso,
                        IdentificadorEjemplar = s.IdentificadorEjemplar,
                        FechaSolicitud = s.FechaSolicitud,
                        Estado = s.Estado
                    }).ToList()
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar todas las solicitudes.");
                TempData["Error"] = "No se pudo cargar el listado de solicitudes.";
                return View(new SolicitudIndexViewModel());
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Pendientes()
        {
            try
            {
                var respuesta = await _gestionPrestamos.ConsultarSolicitudesPendientesAsync();

                var modelo = new SolicitudIndexViewModel
                {
                    Solicitudes = respuesta.Select(s => new SolicitudItemViewModel
                    {
                        SolicitudId = s.SolicitudId,
                        TituloRecurso = s.TituloRecurso,
                        IdentificadorEjemplar = s.IdentificadorEjemplar,
                        FechaSolicitud = s.FechaSolicitud,
                        Estado = s.Estado
                    }).ToList()
                };

                return View("Index", modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar solicitudes pendientes.");
                TempData["Error"] = "No se pudieron cargar las solicitudes pendientes.";
                return RedirectToAction(nameof(Index));
            }
        }

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
        public IActionResult Solicitar(int? ejemplarId)
        {
            var modelo = new RegistrarSolicitudViewModel();

            if (ejemplarId.HasValue && ejemplarId.Value > 0)
            {
                modelo.EjemplarId = ejemplarId.Value;
            }

            // CORREGIDO: Se retorna la variable 'modelo' poblada en lugar de una instancia vacía.
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

        [Authorize(Roles = "Bibliotecario,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int id)
        {
            try
            {
                var request = new AprobarSolicitudRequest { SolicitudId = id };
                await _gestionPrestamos.AprobarPrestamoAsync(request, ObtenerUsuarioId());

                TempData["Success"] = "Solicitud aprobada y préstamo generado exitosamente.";
                return RedirectToAction(nameof(Pendientes));
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Pendientes));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aprobar la solicitud {Id}", id);
                TempData["Error"] = "Ocurrió un error al procesar la aprobación.";
                return RedirectToAction(nameof(Pendientes));
            }
        }
    }
}