using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public class RecursoBibliograficoController : BaseController
    {
        private readonly IGestionCatalogo _gestionCatalogo;
        private readonly IWebHostEnvironment _env;

        public RecursoBibliograficoController(IGestionCatalogo gestionCatalogo, IWebHostEnvironment env)
        {
            _gestionCatalogo = gestionCatalogo;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filtroBusqueda = "")
        {
            var request = new ConsultarCatalogoRequest
            {
                Titulo = filtroBusqueda,
                Autor = filtroBusqueda,
                Categoria = filtroBusqueda
            };

            var recursos = await _gestionCatalogo.ConsultarCatalogoAsync(request);
            return View(recursos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RegistrarRecursoRequest request, IFormFile? imagenPortada)
        {
            try
            {
                if (imagenPortada != null && imagenPortada.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "recursos");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagenPortada.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagenPortada.CopyToAsync(fileStream);
                    }
                    request.ImagenUrl = "/images/recursos/" + uniqueFileName;
                }

                int usuarioId = ObtenerUsuarioId();
                await _gestionCatalogo.RegistrarRecursoAsync(request, usuarioId);

                TempData["Success"] = "Recurso registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var request = new ConsultarDetalleRecursoRequest { RecursoBibliograficoId = id };
                var recurso = await _gestionCatalogo.ConsultarDetalleRecursoAsync(request);

                var model = new ActualizarRecursoRequest
                {
                    RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                    Titulo = recurso.Titulo,
                    Autor = recurso.Autor,
                    CategoriaId = recurso.CategoriaId,
                    AnioPublicado = recurso.AnioPublicado,
                    ImagenUrl = recurso.ImagenUrl
                };

                return View(model);
            }
            catch (Exception)
            {
                TempData["Error"] = "No se pudo cargar el recurso para editar.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ActualizarRecursoRequest request, IFormFile? nuevaImagen)
        {
            try
            {
                if (nuevaImagen != null && nuevaImagen.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "recursos");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + nuevaImagen.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await nuevaImagen.CopyToAsync(fileStream);
                    }
                    request.ImagenUrl = "/images/recursos/" + uniqueFileName;
                }

                int usuarioId = ObtenerUsuarioId();
                await _gestionCatalogo.ActualizarRecursoAsync(request, usuarioId);

                TempData["Success"] = "Recurso actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error al actualizar el recurso: " + ex.Message;
                return View(request);
            }
        }
    }
}