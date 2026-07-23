using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
    public class PenalizacionesController : BaseController
    {
        private readonly IGestionPenalizaciones _gestionPenalizaciones;

        public PenalizacionesController(IGestionPenalizaciones gestionPenalizaciones)
        {
            _gestionPenalizaciones = gestionPenalizaciones;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new ConsultarPenalizacionesRequest();
                int usuarioId = ObtenerUsuarioId();
                var penalizaciones = await _gestionPenalizaciones.ConsultarPenalizacionesAsync(request, usuarioId);
                return View(penalizaciones);
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Error al cargar las penalizaciones: " + ex.Message;
                return View(new List<SIGEBI.Application.DTOs.Response.PenalizacionResponse>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> Resolver(int penalizacionId, string motivoResolucion)
        {
            try
            {
                var request = new ResolverPenalizacionRequest
                {
                    PenalizacionId = penalizacionId,
                    MotivoResolucion = motivoResolucion
                };

                int usuarioId = ObtenerUsuarioId();
                await _gestionPenalizaciones.ResolverPenalizacionAsync(request, usuarioId);

                TempData["Success"] = "Penalización resuelta correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}