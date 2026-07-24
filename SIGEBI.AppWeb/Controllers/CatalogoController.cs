using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Catalogo;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class CatalogoController : BaseController
    {
        private readonly IGestionCatalogo _gestionCatalogo;
        private readonly IGestionCategorias _gestionCategorias;
        private readonly IStorageService _storageService;
        private readonly ILogger<CatalogoController> _logger;

        public CatalogoController(
            IGestionCatalogo gestionCatalogo,
            IGestionCategorias gestionCategorias,
            IStorageService storageService,
            ILogger<CatalogoController> logger)
        {
            _gestionCatalogo = gestionCatalogo;
            _gestionCategorias = gestionCategorias;
            _storageService = storageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ConsultarCatalogoRequest filtro)
        {
            try
            {
                var resultados = await _gestionCatalogo.ConsultarCatalogoAsync(filtro);

                // Retorna la colección IEnumerable<RecursoResponse> que espera la vista Index.cshtml
                return View(resultados);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return View(Enumerable.Empty<RecursoResponse>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el catálogo de recursos.");
                TempData["Error"] = "Ocurrió un error al cargar el catálogo de recursos.";
                return View(Enumerable.Empty<RecursoResponse>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var request = new ConsultarDetalleRecursoRequest { RecursoBibliograficoId = id };
                var detalle = await _gestionCatalogo.ConsultarDetalleRecursoAsync(request);
                return View(detalle);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles del recurso {Id}", id);
                TempData["Error"] = "Ocurrió un error al cargar los detalles del recurso.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            try
            {
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar las categorías para el registro.");
            }

            return View(new RegistrarRecursoViewModel());
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RegistrarRecursoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(model);
            }

            try
            {
                string? imagenUrl = null;

                // Guardado físico de la imagen en wwwroot/imagenes/ de la AppWeb
                if (model.Imagen != null && model.Imagen.Length > 0)
                {
                    using var stream = model.Imagen.OpenReadStream();
                    var extension = Path.GetExtension(model.Imagen.FileName);

                    imagenUrl = await _storageService.GuardarAsync(stream, extension, "imagenes");
                }

                var request = new RegistrarRecursoRequest
                {
                    ISBN = model.ISBN,
                    Titulo = model.Titulo,
                    Autor = model.Autor,
                    CategoriaId = model.CategoriaId,
                    AnioPublicado = model.AnioPublicado,
                    CantidadEjemplares = model.CantidadEjemplares,
                    ImagenUrl = imagenUrl
                };

                await _gestionCatalogo.RegistrarRecursoAsync(request, ObtenerUsuarioId());

                TempData["Success"] = "Recurso bibliográfico registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Advertencia de negocio al registrar recurso: {Mensaje}", ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar el recurso.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al registrar el recurso.");
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(model);
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var request = new ConsultarDetalleRecursoRequest { RecursoBibliograficoId = id };
                var recurso = await _gestionCatalogo.ConsultarDetalleRecursoAsync(request);

                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();

                var model = new ActualizarRecursoViewModel
                {
                    RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                    ISBN = recurso.ISBN,
                    Titulo = recurso.Titulo,
                    Autor = recurso.Autor,
                    CategoriaId = recurso.CategoriaId,
                    AnioPublicado = recurso.AnioPublicado,
                    ImagenUrlActual = recurso.ImagenUrl
                };

                return View(model);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el recurso {Id} para edición.", id);
                TempData["Error"] = "Ocurrió un error al cargar el recurso para edición.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ActualizarRecursoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(model);
            }

            try
            {
                string? imagenUrl = model.ImagenUrlActual;

                // Si se adjuntó una nueva imagen en la edición
                if (model.NuevaImagen != null && model.NuevaImagen.Length > 0)
                {
                    using var stream = model.NuevaImagen.OpenReadStream();
                    var extension = Path.GetExtension(model.NuevaImagen.FileName);
                    imagenUrl = await _storageService.GuardarAsync(stream, extension, "imagenes");
                }

                var request = new ActualizarRecursoRequest
                {
                    RecursoBibliograficoId = model.RecursoBibliograficoId,
                    Titulo = model.Titulo,
                    Autor = model.Autor,
                    CategoriaId = model.CategoriaId,
                    AnioPublicado = model.AnioPublicado,
                    ImagenUrl = imagenUrl
                };

                await _gestionCatalogo.ActualizarRecursoAsync(request, ObtenerUsuarioId());

                TempData["Success"] = "Recurso bibliográfico actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Advertencia de negocio al actualizar el recurso {Id}: {Mensaje}", model.RecursoBibliograficoId, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el recurso {Id}.", model.RecursoBibliograficoId);
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al actualizar el recurso.");
                ViewBag.Categorias = await _gestionCategorias.ConsultarCategoriasAsync();
                return View(model);
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoEjemplar(CambiarEstadoRecursoRequest request)
        {
            try
            {
                await _gestionCatalogo.CambiarEstadoRecursoAsync(request, ObtenerUsuarioId());
                TempData["Success"] = "Estado del ejemplar actualizado correctamente.";
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado del ejemplar {EjemplarId}", request.EjemplarId);
                TempData["Error"] = "Ocurrió un error inesperado al cambiar el estado del ejemplar.";
            }

            return RedirectToAction(nameof(Detalles), new { id = request.RecursoBibliograficoId });
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int recursoBibliograficoId, string motivo)
        {
            try
            {
                var request = new EliminarRecursoRequest
                {
                    RecursoBibliograficoId = recursoBibliograficoId,
                    Motivo = motivo
                };

                await _gestionCatalogo.EliminarRecursoAsync(request, ObtenerUsuarioId());
                TempData["Success"] = "El recurso fue eliminado del catálogo correctamente.";
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el recurso {Id}", recursoBibliograficoId);
                TempData["Error"] = "Ocurrió un error inesperado al intentar eliminar el recurso.";
            }

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
        [HttpGet]
        public async Task<IActionResult> Historial(int id)
        {
            try
            {
                var request = new ConsultarHistorialRecursoRequest { RecursoBibliograficoId = id };
                var historial = await _gestionCatalogo.ConsultarHistorialRecursoAsync(request);
                return View(historial);
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el historial del recurso {Id}", id);
                TempData["Error"] = "Ocurrió un error al cargar el historial del recurso.";
                return RedirectToAction(nameof(Index));
            }
        }

       
    }
}