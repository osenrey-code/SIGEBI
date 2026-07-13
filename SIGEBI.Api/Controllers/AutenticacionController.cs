using SIGEBI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;


namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/acount")]
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
