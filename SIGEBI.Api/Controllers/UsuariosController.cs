using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;


namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
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
            var usuario = await _gestionUsuarios.RegistrarUsuarioAsync(request, actorId);
            return StatusCode(201, usuario);
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> Actualizar([FromRoute]int id, [FromBody] ActualizarUsuarioRequest request)
        {
            //Mientras tanto, luego se sacara del jwt
            int actorId = 1;

            var resultado = await _gestionUsuarios.ActualizarUsuarioAsync(request,id, actorId);
            return Ok(resultado);
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar([FromRoute] int id, [FromBody] DesactivarUsuarioRequest request)
        {
            int actorId = 1;
            await _gestionUsuarios.DesactivarUsuarioAsync(request, id, actorId);

            return Ok(new{ mensaje = "Usuario desactivado correctamente."});
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> Consultar([FromQuery] ConsultarUsuariosRequest request)
        {
            var resultado = await _gestionUsuarios.ConsultarUsuariosAsync(request);
            return Ok(resultado);
        }

        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            int actorId = 1;
            await _gestionUsuarios.ActivarUsuarioAsync(id, actorId);

            return Ok(new
            {
                mensaje = "Usuario activado correctamente."
            });
        }

        [HttpPatch("{id}/cambiar-password")]
        public async Task<IActionResult> CambiarPassword([FromRoute] int id, [FromBody] CambiarPasswordRequest request)
        {
            await _gestionUsuarios.CambiarPasswordAsync(request, id);
            return Ok(new { Mensaje = "Contraseña actualizada correctamente." });
        }
    }
}
