using SIGEBI.Application.DTOs;
using SIGEBI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using System.Runtime.InteropServices;

namespace SIGEBI.Api.Controllers
{
    [Route("api/devolucion")]
    [ApiController]
    public class DevolucionController : ControllerBase
    {
        private readonly IGestionDevolucionesUseCase _devoluciones;

        public DevolucionController(IGestionDevolucionesUseCase devoluciones)
        {
            _devoluciones = devoluciones;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionRequest request)
        {
            int actorId = 1;
            var devolucion = await _devoluciones.RegistrarDevolucionAsync(request, actorId);
            return StatusCode(201, devolucion);
        }

        [HttpGet("historial")]
        public async Task<IActionResult> HistorialDevoluciones([FromBody] ConsultarHistorialDevolucionesRequest request)
        {
            var historial = await _devoluciones.ConsultarHistorialAsync(request);
            return Ok(historial);
        }

    }
}
