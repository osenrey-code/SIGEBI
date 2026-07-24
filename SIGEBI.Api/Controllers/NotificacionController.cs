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
        public async Task<IActionResult> Consultar()
        {
            int usuarioId = ObtenerUsuarioId();

            var notificaciones = await _servicioNotificacion.ObtenerPendientesAsync(usuarioId);

            return Ok(notificaciones);
        }
    }
}