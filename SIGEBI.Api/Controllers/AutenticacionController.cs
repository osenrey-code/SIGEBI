using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacionController : ControllerBase
    {
        [HttpGet("estado")]
        public IActionResult Estado()
        {
            return Ok(new { mensaje = "Controlador de autenticación conectado." });
        }
    }
}
