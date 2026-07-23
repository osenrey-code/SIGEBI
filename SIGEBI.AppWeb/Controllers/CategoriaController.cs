using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public class CategoriaController : BaseController
    {
        private readonly IGestionCategorias _gestionCategorias;

        public CategoriaController(IGestionCategorias gestionCategorias)
        {
            _gestionCategorias = gestionCategorias;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _gestionCategorias.ConsultarCategoriasAsync();
            return View(categorias);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CategoriaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                int actorId = ObtenerUsuarioId();
                await _gestionCategorias.RegistrarCategoriaAsync(request, actorId);

                TempData["Success"] = "Categoría registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(request);
            }
        }
    }
}Si