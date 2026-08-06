using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.UseCase.Usuarios;


namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
   
    public class AutenticacionController : ControllerBase
    {
        private readonly ILogin _login;
        private readonly IGestionUsuariosUseCase _gestionUsuarios;

        public AutenticacionController(ILogin login, IGestionUsuariosUseCase gestionUsuarios)
        {
            _login = login;
            _gestionUsuarios = gestionUsuarios;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Autenticar([FromBody] SIGEBI.Application.DTOs.Request.LoginRequest request)
        {
            var Token = await _login.AutenticarAsync(request);
            return Ok(Token);
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<IActionResult> Registrarse([FromBody] RegistrarUsuarioRequest request)
        {
            var usuario = await _gestionUsuarios.RegistrarUsuarioPublicoAsync(request);
            return StatusCode(201, usuario);
        }
    }
}
