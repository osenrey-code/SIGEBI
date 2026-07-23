using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public class DevolucionesController : BaseController
    {
        private readonly IGestionDevolucionesUseCase _devoluciones;

        public DevolucionesController(IGestionDevolucionesUseCase devoluciones)
        {
            _devoluciones = devoluciones;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new ConsultarHistorialDevolucionesRequest();
                var historial = await _devoluciones.ConsultarHistorialAsync(request);
                return View(historial);
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Error al cargar el historial: " + ex.Message;
                return View(new List<SIGEBI.Application.DTOs.Response.DevolucionResponse>());
            }
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarDevolucionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                int bibliotecarioId = ObtenerUsuarioId();
                var resultado = await _devoluciones.RegistrarDevolucionAsync(request, bibliotecarioId);

                TempData["Success"] = resultado.Mensaje;
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(request);
            }
        }
    }
}