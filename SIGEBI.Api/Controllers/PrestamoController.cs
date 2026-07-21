using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/prestamos")]
    [Authorize]
    public class PrestamoController : BaseApiController
    {
        private readonly IGestionPrestamos _prestamos;

        public PrestamoController(IGestionPrestamos prestamos)
        {
            _prestamos = prestamos;
        }

        [HttpPost("solicitar")]
        [Authorize(Roles = "Docente,Estudiante")]
        public async Task<IActionResult> SolicitarPrestamo([FromBody] RegistrarSolicitudRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            var prestamo = await _prestamos.SolicitarPrestamoAsync(request, usuarioId);
            return Ok(prestamo);
        }

        [HttpPost("aprobar")]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> AprobarPrestamo([FromBody] AprobarSolicitudRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            await _prestamos.AprobarPrestamoAsync(request, usuarioId);
            return Ok(new { Mensaje = "Prestamo aprobado de manera exitosa." });
        }

        [HttpGet("historial")]
        [Authorize(Roles = "Docente,Administrador,Auditor")]
        public async Task<IActionResult> HistorialPrestamo([FromQuery] ConsultarHistorialPrestamosRequest request)
        {
            var historial = await _prestamos.ConsultarHistorialAsync(request);
            return Ok(historial);
        }

        [HttpGet("consultar/activos")]
        [Authorize(Roles = "Bibliotecario,Administrador,Estudiante,Docente")]
        public async Task<IActionResult> ConsultarActivos([FromQuery] ConsultarPrestamosActivosRequest request)
        {
            var activos = await _prestamos.ConsultarPrestamosActivosAsync(request);
            return Ok(activos);
        }
    }
}
