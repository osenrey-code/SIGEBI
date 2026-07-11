using SIGEBI.Domain.Exceptions;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        [HttpGet("estado")]
        public IActionResult GetResultado()
        {
            return Ok(new { mensaje = "!EL controlador de Préstamo esta conectado a SIGEBI." });
        }
    }
}
