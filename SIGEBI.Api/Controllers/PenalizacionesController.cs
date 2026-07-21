using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/penalizaciones")]
    [Authorize]
    public class PenalizacionesController : BaseApiController
    {
        private readonly IGestionPenalizaciones _gestionPenalizaciones;

        public PenalizacionesController(
            IGestionPenalizaciones gestionPenalizaciones)
        {
            _gestionPenalizaciones = gestionPenalizaciones;
        }

        [HttpGet("consultar")]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
        public async Task<IActionResult> ConsultarPenalizaciones(
            [FromQuery] ConsultarPenalizacionesRequest request)
        {
            int usuarioEjecutorId = ObtenerUsuarioId();

            var penalizaciones = await _gestionPenalizaciones
                .ConsultarPenalizacionesAsync(
                    request,
                    usuarioEjecutorId
                );

            return Ok(penalizaciones);
        }

        // GET: api/penalizaciones/activas?usuarioId=2
        [HttpGet("consultar/activas")]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
        public async Task<IActionResult> ConsultarPenalizacionesActivas(
            [FromQuery] ConsultarPenalizacionesActivasRequest request)
        {
            int usuarioEjecutorId = ObtenerUsuarioId();

            var penalizaciones = await _gestionPenalizaciones
                .ConsultarPenalizacionesActivasAsync(
                    request,
                    usuarioEjecutorId
                );

            return Ok(penalizaciones);
        }

        // PATCH: api/penalizaciones/resolver
        [HttpPatch("resolver")]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> ResolverPenalizacion(
            [FromBody] ResolverPenalizacionRequest request)
        {
            int usuarioEjecutorId = ObtenerUsuarioId();

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