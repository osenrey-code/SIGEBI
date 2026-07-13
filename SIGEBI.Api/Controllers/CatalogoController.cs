using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public class CatalogoController : ControllerBase
    {
        private readonly IGestionCatalogo _gestionCatalogo;

        public CatalogoController(IGestionCatalogo gestionCatalogo)
        {
            _gestionCatalogo = gestionCatalogo;
        }

        // POST: api/catalogo/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarRecurso(
            [FromBody] RegistrarRecursoRequest request)
        {
            int usuarioEjecutorId = 1;

            var recurso = await _gestionCatalogo.RegistrarRecursoAsync(
                request,
                usuarioEjecutorId
            );

            return CreatedAtAction(
                nameof(ConsultarDetalle),
                new { id = recurso.RecursoBibliograficoId },
                new
                {
                    Mensaje = "Recurso registrado correctamente.",
                    Recurso = recurso
                }
            );
        }

        // PUT: api/catalogo/actualizar
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarRecurso(
            [FromBody] ActualizarRecursoRequest request)
        {
            int usuarioEjecutorId = 1;

            var recurso = await _gestionCatalogo.ActualizarRecursoAsync(
                request,
                usuarioEjecutorId
            );

            return Ok(new
            {
                Mensaje = "Recurso actualizado correctamente.",
                Recurso = recurso
            });
        }

        // PATCH: api/catalogo/cambiar-estado
        [HttpPatch("cambiar-estado")]
        public async Task<IActionResult> CambiarEstadoRecurso(
            [FromBody] CambiarEstadoRecursoRequest request)
        {
            int usuarioEjecutorId = 1;

            var recurso = await _gestionCatalogo.CambiarEstadoRecursoAsync(
                request,
                usuarioEjecutorId
            );

            return Ok(new
            {
                Mensaje = "Estado del ejemplar actualizado correctamente.",
                Recurso = recurso
            });
        }

        // GET: api/catalogo/consultar?titulo=...&autor=...&categoria=...
        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarCatalogo(
            [FromQuery] ConsultarCatalogoRequest request)
        {
            var recursos = await _gestionCatalogo.ConsultarCatalogoAsync(
                request
            );

            return Ok(recursos);
        }

        // GET: api/catalogo/todos
        [HttpGet("todos")]
        public async Task<IActionResult> ConsultarTodos()
        {
            var recursos = await _gestionCatalogo.ConsultarTodosAsync();

            return Ok(recursos);
        }

        // GET: api/catalogo/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ConsultarDetalle(int id)
        {
            var request = new ConsultarDetalleRecursoRequest
            {
                RecursoBibliograficoId = id
            };

            var recurso = await _gestionCatalogo
                .ConsultarDetalleRecursoAsync(request);

            return Ok(recurso);
        }

        // GET: api/catalogo/5/historial
        [HttpGet("{id:int}/historial")]
        public async Task<IActionResult> ConsultarHistorial(int id)
        {
            var request = new ConsultarHistorialRecursoRequest
            {
                RecursoBibliograficoId = id
            };

            var historial = await _gestionCatalogo
                .ConsultarHistorialRecursoAsync(request);

            return Ok(historial);
        }

        // DELETE: api/catalogo/5?motivo=Recurso deteriorado
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarRecurso(
            int id,
            [FromQuery] string? motivo)
        {
            int usuarioEjecutorId = 1;

            var request = new EliminarRecursoRequest
            {
                RecursoBibliograficoId = id,
                Motivo = motivo
            };

            await _gestionCatalogo.EliminarRecursoAsync(
                request,
                usuarioEjecutorId
            );

            return Ok(new
            {
                Mensaje = "Recurso eliminado correctamente."
            });
        }
    }
}