using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacionController : BaseApiController
    {
        private readonly IServicioNotificacion _servicioNotificacion;

        public NotificacionController(IServicioNotificacion servicioNotificacion)
        {
            _servicioNotificacion = servicioNotificacion;
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> Consultar([FromQuery] bool soloNoLeidas = false)
        {
            int usuarioId = ObtenerUsuarioId();

            if (soloNoLeidas)
            {
                // Devuelve únicamente las pendientes/no leídas
                var pendientes = await _servicioNotificacion.ObtenerPendientesAsync(usuarioId);
                return Ok(pendientes);
            }

            // Devuelve el historial completo (leídas y no leídas)
            var todas = await _servicioNotificacion.ObtenerTodasAsync(usuarioId);
            return Ok(todas);
        }

        [HttpPost("marcarleida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            await _servicioNotificacion.MarcarComoLeidaAsync(id);

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _servicioNotificacion.EliminarAsync(id);

            return Ok();
        }
    }
}