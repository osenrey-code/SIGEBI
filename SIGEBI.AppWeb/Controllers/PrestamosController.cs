using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.AppWeb.Models.Prestamos;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class PrestamosController : BaseController
    {
        private readonly IGestionPrestamos _prestamos;

        public PrestamosController(IGestionPrestamos prestamos)
        {
            _prestamos = prestamos;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string identificacionBusqueda = "")
        {
            try
            {
                var request = new ConsultarPrestamosActivosRequest
                {
                    Identificacion = identificacionBusqueda
                };

                var activos = await _prestamos.ConsultarPrestamosActivosAsync(request);

                var modelo = new PrestamoIndexViewModel
                {
                    Prestamos = activos.Select(p => new PrestamoItemViewModel
                    {
                        PrestamoId = p.PrestamoId,
                        TituloRecurso = p.TituloRecurso,
                        IdentificadorEjemplar = p.IdentificadorEjemplar,
                        FechaInicio = p.FechaInicio,
                        FechaLimite = p.FechaLimite,
                        Estado = p.Estado,
                        EstaVencido = p.EstaVencido
                    }).ToList()
                };

                return View(modelo);
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new PrestamoIndexViewModel());
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> Solicitudes()
        {
            try
            {
                var pendientes = await _prestamos.ConsultarSolicitudesPendientesAsync();
                return View(pendientes);
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "No se pudieron cargar las solicitudes: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> Aprobar(int solicitudId)
        {
            try
            {
                var request = new AprobarSolicitudRequest { SolicitudId = solicitudId };
                int usuarioId = ObtenerUsuarioId();

                await _prestamos.AprobarPrestamoAsync(request, usuarioId);

                TempData["Success"] = "Préstamo aprobado correctamente.";
                return RedirectToAction(nameof(Solicitudes));
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Solicitudes));
            }
        }
    }
}