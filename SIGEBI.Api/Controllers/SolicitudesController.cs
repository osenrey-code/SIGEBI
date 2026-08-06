using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/solicitudes")]
    [Authorize]
    public class SolicitudesController : BaseApiController
    {
        private readonly IGestionPrestamos _prestamos;

        public SolicitudesController(IGestionPrestamos prestamos)
        {
            _prestamos = prestamos;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> ConsultarTodas()
        {
            var respuesta = await _prestamos.ConsultarTodasAsync();
            return Ok(respuesta);
        }

        [HttpGet("pendientes")]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> ConsultarPendientes()
        {
            var respuesta = await _prestamos.ConsultarSolicitudesPendientesAsync();
            return Ok(respuesta);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> ObtenerDetalles(int id)
        {
            var detalle = await _prestamos.ObtenerPorIdConDetallesAsync(id);
            if (detalle == null)
            {
                return NotFound(new { Mensaje = "La solicitud especificada no existe." });
            }
            return Ok(detalle);
        }

        [HttpPost("aprobar")]
        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Aprobar([FromBody] AprobarSolicitudRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            await _prestamos.AprobarPrestamoAsync(request, usuarioId);
            return Ok(new { Mensaje = "Solicitud aprobada y préstamo generado exitosamente." });
        }

        [HttpPost("rechazar")]
        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Rechazar([FromBody] RechazarSolicitudRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            var resultado = await _prestamos.RechazarSolicitudAsync(request, usuarioId);
            return Ok(new
            {
                Mensaje = "Solicitud Rechazada exitosamente.",
                Solicitud = resultado
            });
        }
    }
}