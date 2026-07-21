using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;


namespace SIGEBI.Api.Controllers
{
    [Route("api/devolucion")]
    [ApiController]
    [Authorize]
    public class DevolucionController : BaseApiController
    {
        private readonly IGestionDevolucionesUseCase _devoluciones;

        public DevolucionController(IGestionDevolucionesUseCase devoluciones)
        {
            _devoluciones = devoluciones;
        }

        [HttpPost("registrar")]
        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionRequest request)
        {
            int actorId = ObtenerUsuarioId();
            var devolucion = await _devoluciones.RegistrarDevolucionAsync(request, actorId);
            return StatusCode(201, devolucion);
        }

        [HttpGet("historial")]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
        public async Task<IActionResult> HistorialDevoluciones([FromQuery] ConsultarHistorialDevolucionesRequest request)
        {
            var historial = await _devoluciones.ConsultarHistorialAsync(request);
            return Ok(historial);
        }

    }
}
