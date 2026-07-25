using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Service;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize] // Exigimos que el usuario haya iniciado sesión
    public class CategoriasController : Controller
    {
        private readonly IGestionCategorias _gestionCategorias;

        public CategoriasController(IGestionCategorias gestionCategorias)
        {
            _gestionCategorias = gestionCategorias;
        }

        // --- GET: /Categorias/Index ---
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtenemos todas las categorías directo del caso de uso
                var categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(categorias);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(new List<CategoriaResponse>());
            }
        }

        // --- POST: /Categorias/Registrar ---
        [HttpPost]
        [Authorize(Roles = "Administrador,Bibliotecario")] // Validado aquí y en tu UseCase
        public async Task<IActionResult> Registrar(CategoriaRequest request)
        {
            try
            {
                // Extraemos el ID del usuario actual desde las cookies/claims
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Ejecutamos el caso de uso directamente
                await _gestionCategorias.RegistrarCategoriaAsync(request, currentUserId);

                TempData["SuccessMessage"] = $"La categoría '{request.Nombre}' se registró exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al registrar la categoría: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}