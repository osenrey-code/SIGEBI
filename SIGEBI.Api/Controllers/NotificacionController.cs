using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacionController : ControllerBase
    {

        [HttpGet("consultar")] 
        public async Task<IActionResult> Consultar()
        {
            var notificacionesPrueba = new List<object>
            {
                new { NotificacionId = 1, Tipo = "Préstamo", Mensaje = "Tu libro vence mañana.", FechaRegistro = DateTime.Now.AddHours(-2), Leida = false },
                new { NotificacionId = 2, Tipo = "Sistema", Mensaje = "Bienvenido al sistema SIGEBI.", FechaRegistro = DateTime.Now.AddDays(-1), Leida = true }
            };

            return Ok(notificacionesPrueba);
        }
    }
}