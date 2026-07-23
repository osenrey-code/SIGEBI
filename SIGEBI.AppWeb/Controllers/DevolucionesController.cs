using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Devoluciones;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{

    [Authorize(Roles = "Administrador,Auditor,Bibliotecario")]
    public class DevolucionesController : BaseController
    {
        private readonly IGestionDevolucionesUseCase _gestionDevoluciones;
        private readonly ILogger<DevolucionesController> _logger;

        public DevolucionesController(
            IGestionDevolucionesUseCase gestionDevoluciones,
            ILogger<DevolucionesController> logger)
        {
            _gestionDevoluciones = gestionDevoluciones;
            _logger = logger;
        }

 
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] HistorialDevolucionesViewModel modelo)
        {
            try
            {
                var request = new ConsultarHistorialDevolucionesRequest
                {
                    UsuarioId = modelo.UsuarioId,
                    RecursoBibliograficoId = modelo.RecursoBibliograficoId,
                    EjemplarId = modelo.EjemplarId,
                    FechaInicio = modelo.FechaInicio,
                    FechaFin = modelo.FechaFin
                };

                modelo.Devoluciones = await _gestionDevoluciones.ConsultarHistorialAsync(request);
                return View(modelo);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                modelo.Devoluciones = new List<Application.DTOs.Response.DevolucionResponse>();
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el historial de devoluciones.");
                TempData["Error"] = "Ocurrió un error al cargar el historial de devoluciones.";
                modelo.Devoluciones = new List<Application.DTOs.Response.DevolucionResponse>();
                return View(modelo);
            }
        }

        [Authorize(Roles = "Bibliotecario")]
        [HttpGet]
        public IActionResult Registrar(int prestamoId)
        {
            if (prestamoId <= 0)
            {
                TempData["Error"] = "Debe especificar un ID de préstamo válido para registrar la devolución.";
                return RedirectToAction(nameof(Index));
            }

            var modelo = new RegistrarDevolucionViewModel
            {
                PrestamoId = prestamoId
            };

            return View(modelo);
        }


        [Authorize(Roles = "Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarDevolucionViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                var request = new RegistrarDevolucionRequest
                {
                    PrestamoId = modelo.PrestamoId,
                    Condicion = modelo.Condicion,
                    Observacion = modelo.Observacion
                };

                // Obtenemos el ID del usuario logueado usando el método base del sistema
                int bibliotecarioId = ObtenerUsuarioId();

                var resultado = await _gestionDevoluciones.RegistrarDevolucionAsync(request, bibliotecarioId);

                TempData["Success"] = resultado.Mensaje;
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la devolución para el préstamo ID {PrestamoId}.", modelo.PrestamoId);
                TempData["Error"] = "Ocurrió un error inesperado al procesar la devolución en el sistema.";
                return View(modelo);
            }
        }
    }
}