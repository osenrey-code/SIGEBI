using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/penalizaciones")]
    public class PenalizacionesController : ControllerBase
    {
        private readonly IGestionPenalizaciones _gestionPenalizaciones;

        public PenalizacionesController(
            IGestionPenalizaciones gestionPenalizaciones)
        {
            _gestionPenalizaciones = gestionPenalizaciones;
        }

        // GET: api/penalizaciones
        // GET: api/penalizaciones?usuarioId=2
        // GET: api/penalizaciones?prestamoId=5&estado=Activa
        [HttpGet]
        public async Task<IActionResult> ConsultarPenalizaciones(
            [FromQuery] ConsultarPenalizacionesRequest request)
        {
            int usuarioEjecutorId = 1;

            var penalizaciones = await _gestionPenalizaciones
                .ConsultarPenalizacionesAsync(
                    request,
                    usuarioEjecutorId
                );

            return Ok(penalizaciones);
        }

        // GET: api/penalizaciones/activas?usuarioId=2
        [HttpGet("activas")]
        public async Task<IActionResult> ConsultarPenalizacionesActivas(
            [FromQuery] ConsultarPenalizacionesActivasRequest request)
        {
            int usuarioEjecutorId = 1;

            var penalizaciones = await _gestionPenalizaciones
                .ConsultarPenalizacionesActivasAsync(
                    request,
                    usuarioEjecutorId
                );

            return Ok(penalizaciones);
        }

        // PATCH: api/penalizaciones/resolver
        [HttpPatch("resolver")]
        public async Task<IActionResult> ResolverPenalizacion(
            [FromBody] ResolverPenalizacionRequest request)
        {
            int usuarioEjecutorId = 1;

            var penalizacion = await _gestionPenalizaciones
                .ResolverPenalizacionAsync(
                    request,
                    usuarioEjecutorId
                );

            return Ok(new
            {
                Mensaje = "Penalización resuelta correctamente.",
                Penalizacion = penalizacion
            });
        }
    }
}