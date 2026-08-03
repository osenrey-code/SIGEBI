using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;


namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosController : BaseApiController
    {
        private readonly IGestionUsuariosUseCase _gestionUsuarios;

        public UsuariosController(IGestionUsuariosUseCase gestionUsuarios)
        {
            _gestionUsuarios = gestionUsuarios;
        }

        [HttpPost("registrar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request)
        {
            int actorId = ObtenerUsuarioId();
            var usuario = await _gestionUsuarios.RegistrarUsuarioAsync(request, actorId);
            return StatusCode(201, usuario);
        }

        [HttpPut("{id}/actualizar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Actualizar([FromRoute]int id, [FromBody] ActualizarUsuarioRequest request)
        {
            int actorId = ObtenerUsuarioId();

            var resultado = await _gestionUsuarios.ActualizarUsuarioAsync(request,id, actorId);
            return Ok(resultado);
        }

        [HttpPatch("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Desactivar([FromRoute] int id, [FromBody] DesactivarUsuarioRequest request)
        {
            int actorId = ObtenerUsuarioId();
            await _gestionUsuarios.DesactivarUsuarioAsync(request, id, actorId);

            return Ok(new{ mensaje = "Usuario desactivado correctamente."});
        }

        [HttpGet("consultar")]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> Consultar([FromQuery] ConsultarUsuariosRequest request)
        {
            var resultado = await _gestionUsuarios.ConsultarUsuariosAsync(request);
            return Ok(resultado);
        }

        [HttpPatch("{id}/activar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            int actorId = ObtenerUsuarioId();
            await _gestionUsuarios.ActivarUsuarioAsync(id, actorId);

            return Ok(new
            {
                mensaje = "Usuario activado correctamente."
            });
        }

        [HttpPatch("cambiar-mi-password")]
        public async Task<IActionResult> CambiarMiPassword([FromBody] CambiarPasswordRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            await _gestionUsuarios.CambiarPasswordAsync(request, usuarioId);
            return Ok(new { mensaje = "Tu contraseña ha sido actualizada correctamente." });
        }

        [HttpPatch("{id:int}/resetear-password-admin")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ResetearPasswordAdmin([FromRoute] int id, [FromBody] ResetearPasswordAdminRequest request)
        {
            int actorId = ObtenerUsuarioId();
            await _gestionUsuarios.CambiarPasswordAdminAsync(id, request.NuevaPassword, actorId);
            return Ok(new { mensaje = "La contraseña del usuario ha sido restablecida exitosamente." });
        }

        [HttpGet("perfil")]
        public async Task<IActionResult> ObtenerMiPerfil()
        {
            
            int usuarioId = ObtenerUsuarioId();
            var perfil = await _gestionUsuarios.BuscarPorIdAsync(usuarioId);

            return Ok(perfil);
        }
    }
}
