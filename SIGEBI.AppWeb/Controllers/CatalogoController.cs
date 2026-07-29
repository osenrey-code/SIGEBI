//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SIGEBI.Application.DTOs.Request;
//using SIGEBI.Application.DTOs.Response;
//using SIGEBI.Application.Interfaces.Service;
//using SIGEBI.AppWeb.Models.Catalogo;
//using SIGEBI.Domain.Exceptions;

//namespace SIGEBI.AppWeb.Controllers
//{
//    [Authorize]
//    public class CatalogoController : BaseController
//    {
//        private readonly IGestionCatalogo _gestionCatalogo;
//        private readonly IGestionCategorias _gestionCategorias;
//        private readonly IStorageService _storageService;
//        private readonly ILogger<CatalogoController> _logger;

//        public CatalogoController(
//            IGestionCatalogo gestionCatalogo,
//            IGestionCategorias gestionCategorias,
//            IStorageService storageService,
//            ILogger<CatalogoController> logger)
//        {
//            _gestionCatalogo = gestionCatalogo;
//            _gestionCategorias = gestionCategorias;
//            _storageService = storageService;
//            _logger = logger;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index([FromQuery] ConsultarCatalogoRequest filtro)
//        {
//            try
//            {
//                var resultados = await _gestionCatalogo.ConsultarCatalogoAsync(filtro);

//                // Retorna la colección IEnumerable<RecursoResponse> que espera la vista Index.cshtml
//                return View(resultados);
//            }
//            catch (BusinessException ex)
//            {
//                TempData["Error"] = ex.Message;
//                return View(Enumerable.Empty<RecursoResponse>());
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al consultar el catálogo de recursos.");
//                TempData["Error"] = "Ocurrió un error al cargar el catálogo de recursos.";
//                return View(Enumerable.Empty<RecursoResponse>());
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> Detalles(int id)
//        {
//            try
//            {
//                var request = new ConsultarDetalleRecursoRequest { RecursoBibliograficoId = id };
//                var detalle = await _gestionCatalogo.ConsultarDetalleRecursoAsync(request);
//                return View(detalle);
//            }
//            catch (BusinessException ex)
//            {
//                TempData["Error"] = ex.Message;
//                return RedirectToAction(nameof(Index));
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener los detalles del recurso {Id}", id);
//                TempData["Error"] = "Ocurrió un error al cargar los detalles del recurso.";
//                return RedirectToAction(nameof(Index));
//            }
//        }
//    }
//}