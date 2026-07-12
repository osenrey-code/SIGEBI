using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;


namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IGestionUsuariosUseCase _gestionUsuarios;

        public UsuariosController(IGestionUsuariosUseCase gestionUsuarios)
        {
            _gestionUsuarios = gestionUsuarios;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request)
        {
            int actorId = 1;
            await _gestionUsuarios.RegistrarUsuarioAsync(request, actorId);
            return Ok(new { Mensaje = "Usuario creado correctamente."});
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarUsuarioRequest request)
        {
            //Mientras tanto, luego se sacara del jwt
            int actorId = 2;

            var resultado = await _gestionUsuarios.ActualizarUsuarioAsync(request, actorId);
            return Ok(resultado);
        }

        [HttpPatch("desactivar")]
        public async Task<IActionResult> Desactivar([FromBody] DesactivarUsuarioRequest request)
        {
            int actorId = 1;
            await _gestionUsuarios.DesactivarUsuarioAsync(request, actorId);

            return Ok(new
            {
                mensaje = "Usuario desactivado correctamente."
            });
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> Consultar([FromQuery] ConsultarUsuariosRequest request)
        {
            var resultado = await _gestionUsuarios.ConsultarUsuariosAsync(request);
            return Ok(resultado);
        }

        [HttpPatch("activar")]
        public async Task<IActionResult> Activar([FromBody] ActivarUsuarioRequest request)
        {
            int actorId = 1;
            await _gestionUsuarios.ActivarUsuarioAsync(request, actorId);

            return Ok(new
            {
                mensaje = "Usuario activado correctamente."
            });
        }

        [HttpPatch("cambiar-password")]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordRequest request)
        {
            await _gestionUsuarios.CambiarPasswordAsync(request);
            return Ok(new { Mensaje = "Contraseña actualizada correctamente." });
        }
    }
}
