using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Response;
using Microsoft.AspNetCore.Identity.Data;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly ILogin _login;

        public AutenticacionController(ILogin login)
        {
            _login = login;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Autenticar([FromBody] SIGEBI.Application.DTOs.Request.LoginRequest request)
        {
            var Token = await _login.AutenticarAsync(request);
            return Ok(Token);
        }
    }
}
