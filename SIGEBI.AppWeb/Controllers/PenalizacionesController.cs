using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Service;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
    public class PenalizacionesController : Controller
    {
        private readonly IGestionPenalizaciones _gestionPenalizaciones;

        public PenalizacionesController(IGestionPenalizaciones gestionPenalizaciones)
        {
            _gestionPenalizaciones = gestionPenalizaciones;
        }

        // --- GET: /Penalizaciones/Index ---
        public async Task<IActionResult> Index(int? usuarioId, string? estado)
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var request = new ConsultarPenalizacionesRequest
                {
                    UsuarioId = usuarioId,
                    Estado = estado
                };

                var penalizaciones = await _gestionPenalizaciones.ConsultarPenalizacionesAsync(request, currentUserId);

                ViewBag.EstadoActual = estado;
                ViewBag.UsuarioFiltro = usuarioId;

                return View(penalizaciones);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(new List<PenalizacionResponse>());
            }
        }

        // --- POST: /Penalizaciones/Resolver ---
        [HttpPost]
        [Authorize(Roles = "Administrador,Bibliotecario")] 
        public async Task<IActionResult> Resolver(int penalizacionId, string motivoResolucion)
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Preparamos el DTO de resolución
                var request = new ResolverPenalizacionRequest
                {
                    PenalizacionId = penalizacionId,
                    MotivoResolucion = motivoResolucion
                };

                // Ejecutamos el UseCase
                await _gestionPenalizaciones.ResolverPenalizacionAsync(request, currentUserId);

                TempData["SuccessMessage"] = $"Penalización #{penalizacionId} resuelta correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al resolver la penalización: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}