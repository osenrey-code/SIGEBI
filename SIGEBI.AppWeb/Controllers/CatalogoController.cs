using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Services;
using SIGEBI.AppWeb.Models.DTOs.Catalogo;
using SIGEBI.AppWeb.Models.ViewModels.Catalogo;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Estudiante,Docente")]
    public class CatalogoController : BaseController
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<CatalogoController> _logger;

        public CatalogoController(
            IApiClient apiclient,
            ILogger<CatalogoController> logger)
        {
            _apiClient = apiclient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ConsultarCatalogoRequest filtro)
        {
            var viewModel = new CatalogoIndexViewModel
            {
                Titulo = filtro.Titulo,
                Autor = filtro.Autor,
                Categoria = filtro.Categoria,
                SoloDisponibles = filtro.SoloDisponibles ??  false
            };

            try
            {
                var queryParams = new List<string>();

                if (!string.IsNullOrWhiteSpace(filtro.Titulo))
                    queryParams.Add($"titulo={Uri.EscapeDataString(filtro.Titulo)}");

                if (!string.IsNullOrWhiteSpace(filtro.Autor))
                    queryParams.Add($"autor={Uri.EscapeDataString(filtro.Autor)}");

                if (!string.IsNullOrWhiteSpace(filtro.Categoria))
                    queryParams.Add($"categoria={Uri.EscapeDataString(filtro.Categoria)}");

                if (filtro.SoloDisponibles.HasValue && filtro.SoloDisponibles.Value)
                    queryParams.Add("soloDisponibles=true");

                string endpoint;
                if (queryParams.Any())
                {
                    endpoint = $"api/catalogo/consultar?{string.Join("&", queryParams)}";
                }
                else
                {
                    endpoint = "api/catalogo/todos";
                }

                var respuesta = await _apiClient.GetAsync<List<Recursodto>>(endpoint)
                    ?? new List<Recursodto>();

                viewModel.Recursos = respuesta.Select(r => new RecursoItemViewModel
                {
                    RecursoBibliograficoId = r.RecursoBibliograficoId,
                    ISBN = r.ISBN,
                    Titulo = r.Titulo,
                    Autor = r.Autor,
                    Categoria = r.Categoria,
                    AnioPublicado = r.AnioPublicado,
                    ImagenUrl = r.ImagenUrl,
                    TotalEjemplares = r.TotalEjemplares,
                    CopiasDisponibles = r.CopiasDisponibles
                }).ToList();

            } catch(Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el catálogo de recursos.");
                TempData["Error"] = ex.Message;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Identificador de recurso no válido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var recurso = await _apiClient.GetAsync<Recursodto>($"api/catalogo/{id}");

                if (recurso == null)
                {
                    TempData["Error"] = "El recurso bibliográfico solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new RecursoItemViewModel
                {
                    RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                    ISBN = recurso.ISBN,
                    Titulo = recurso.Titulo,
                    Autor = recurso.Autor,
                    Categoria = recurso.Categoria,
                    AnioPublicado = recurso.AnioPublicado,
                    ImagenUrl = recurso.ImagenUrl,
                    TotalEjemplares = recurso.TotalEjemplares,
                    CopiasDisponibles = recurso.CopiasDisponibles
                };

                return View(viewModel);

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el detalle del recurso {Id}", id);
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}