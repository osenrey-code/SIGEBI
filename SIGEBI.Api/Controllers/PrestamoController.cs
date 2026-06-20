using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : ControllerBase
    {
        [HttpGet("estado")]
        public IActionResult GetResultado()
        {
            return Ok(new { mensaje = "!EL controlador de Préstamo esta conectado a SIGEBI." });
        }
    }
}
