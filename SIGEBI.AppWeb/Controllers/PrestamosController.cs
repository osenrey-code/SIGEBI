using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.AppWeb.Models.Prestamos;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class PrestamosController : BaseController
    {
        private readonly IGestionPrestamos _gestionPrestamos;
        private readonly ILogger<PrestamosController> _logger;

        public PrestamosController(IGestionPrestamos gestionPrestamos, ILogger<PrestamosController> logger)
        {
            _gestionPrestamos = gestionPrestamos;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Activos));
        }

        [Authorize(Roles = "Administrador,Bibliotecario,Docente,Estudiante")]
        [HttpGet]
        public async Task<IActionResult> Activos(string? identificacion, int? recursoId, int? ejemplarId)
        {
            try
            {
                var request = new ConsultarPrestamosActivosRequest
                {
                    Identificacion = identificacion,
                    RecursoBibliograficoId = recursoId,
                    EjemplarId = ejemplarId
                };

                var respuesta = await _gestionPrestamos.ConsultarPrestamosActivosAsync(request, ObtenerUsuarioId());

                var modelo = new PrestamoFiltroViewModel
                {
                    Identificacion = identificacion,
                    RecursoBibliograficoId = recursoId,
                    EjemplarId = ejemplarId,
                    Prestamos = respuesta.Select(p => new PrestamoItemViewModel
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar los préstamos activos.");
                TempData["Error"] = "No se pudieron cargar los préstamos activos.";
                return View(new PrestamoFiltroViewModel());
            }
        }

        [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
        [HttpGet]
        public async Task<IActionResult> Historial(string? identificacion, int? recursoId, int? ejemplarId)
        {
            try
            {
                var request = new ConsultarHistorialPrestamosRequest
                {
                    Identificacion = identificacion,
                    RecursoBibliograficoId = recursoId,
                    EjemplarId = ejemplarId
                };

                var respuesta = await _gestionPrestamos.ConsultarHistorialAsync(request);

                var modelo = new PrestamoFiltroViewModel
                {
                    Identificacion = identificacion,
                    RecursoBibliograficoId = recursoId,
                    EjemplarId = ejemplarId,
                    Prestamos = respuesta.Select(p => new PrestamoItemViewModel
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el historial de préstamos.");
                TempData["Error"] = "No se pudo cargar el historial de préstamos.";
                return View(new PrestamoFiltroViewModel());
            }
        }
    }
}