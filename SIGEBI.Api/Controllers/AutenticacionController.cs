using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;


namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
   
    public class AutenticacionController : ControllerBase
    {
        private readonly ILogin _login;

        public AutenticacionController(ILogin login)
        {
            _login = login;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Autenticar([FromBody] SIGEBI.Application.DTOs.Request.LoginRequest request)
        {
            var Token = await _login.AutenticarAsync(request);
            return Ok(Token);
        }
    }
}
