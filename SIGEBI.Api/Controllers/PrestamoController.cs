using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : ControllerBase
    {
        private readonly IGestionPrestamos _prestamos;

        public PrestamoController(IGestionPrestamos prestamos)
        {
            _prestamos = prestamos;
        }

        [HttpPost("solicitarPrestamo")]
        public async Task<IActionResult> SolicitarPrestamo([FromBody] RegistrarSolicitudRequest request)
        {
            int usuarioId = 2;
            await _prestamos.SolicitarPrestamoAsync(request, usuarioId);
            return Ok(new { Mensaje = "Solictud realizada correctamente." });
        }

        [HttpPost("aprobarPrestamo")]
        public async Task<IActionResult> AprobarPrestamo([FromBody] AprobarSolicitudRequest request)
        {
            int usuarioId = 1;
            await _prestamos.AprobarPrestamoAsync(request, usuarioId);
            return Ok(new { Mensaje = "Prestamo aprobado de manera exitosa." });
        }

        [HttpGet("historialPrestamos")]
        public async Task<IActionResult> HistorialPrestamo([FromQuery] ConsultarHistorialPrestamosRequest request)
        {
            var historial = await _prestamos.ConsultarHistorialAsync(request);
            return Ok(historial);
        }

        [HttpGet("consultarActivos")]
        public async Task<IActionResult> ConsultarActivos([FromQuery] ConsultarPrestamosActivosRequest request)
        {
            var activos = await _prestamos.ConsultarPrestamosActivosAsync(request);
            return Ok(activos);
        }
    }
}
