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
            var resultado =  await _gestionUsuarios.RegistrarUsuarioAsync(request, actorId);
            return Ok(resultado);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarUsuarioRequest request)
        {
            //Mientras tanto, luego se sacara del jwt
            int actorId = 1;

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
        public async Task<IActionResult> Consultar([FromBody] ConsultarUsuariosRequest request)
        {
            var resultado = await _gestionUsuarios.ConsultarUsuariosAsync(request);
            return Ok(resultado);
        }
    }
}
