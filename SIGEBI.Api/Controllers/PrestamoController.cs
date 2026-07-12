using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/prestamos")]
    [Authorize]
    public class PrestamoController : ControllerBase
    {
        private readonly IGestionPrestamos _prestamos;

        public PrestamoController(IGestionPrestamos prestamos)
        {
            _prestamos = prestamos;
        }

        [HttpPost("solicitar")]
        public async Task<IActionResult> SolicitarPrestamo([FromBody] RegistrarSolicitudRequest request)
        {
            int usuarioId = 2;
            await _prestamos.SolicitarPrestamoAsync(request, usuarioId);
            return Ok(new { Mensaje = "Solictud realizada correctamente." });
        }

        [HttpPost("aprobar/{id}")]
        public async Task<IActionResult> AprobarPrestamo([FromBody] AprobarSolicitudRequest request)
        {
            int usuarioId = 1;
            await _prestamos.AprobarPrestamoAsync(request, usuarioId);
            return Ok(new { Mensaje = "Prestamo aprobado de manera exitosa." });
        }

        [HttpGet("historial")]
        public async Task<IActionResult> HistorialPrestamo([FromQuery] ConsultarHistorialPrestamosRequest request)
        {
            var historial = await _prestamos.ConsultarHistorialAsync(request);
            return Ok(historial);
        }

        [HttpGet("consultar/activos")]
        public async Task<IActionResult> ConsultarActivos([FromQuery] ConsultarPrestamosActivosRequest request)
        {
            var activos = await _prestamos.ConsultarPrestamosActivosAsync(request);
            return Ok(activos);
        }
    }
}
