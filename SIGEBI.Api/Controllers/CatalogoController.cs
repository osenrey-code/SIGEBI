using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    [Authorize]
    public class CatalogoController : BaseApiController
    {
        private readonly IGestionCatalogo _gestionCatalogo;
        private readonly IStorageService _storageService; 

        public CatalogoController(IGestionCatalogo gestionCatalogo, IStorageService storageService)
        {
            _gestionCatalogo = gestionCatalogo;
            _storageService = storageService;
        }

        // POST: api/catalogo/registrar
        [HttpPost("registrar")]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> RegistrarRecurso([FromForm] RegistrarRecursoRequest request)
        {
            if (request.ImagenArchivo != null && request.ImagenArchivo.Length > 0)
            {
                using var stream = request.ImagenArchivo.OpenReadStream();
                var extension = System.IO.Path.GetExtension(request.ImagenArchivo.FileName);
                string rutaImagen = await _storageService.GuardarAsync(stream, extension, "imagenes");

                request.ImagenUrl = rutaImagen;
            }

            int usuarioEjecutorId = ObtenerUsuarioId();

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
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> ActualizarRecurso([FromForm] ActualizarRecursoRequest request)
        {
  
            int usuarioEjecutorId = ObtenerUsuarioId();

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
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> CambiarEstadoRecurso(
            [FromBody] CambiarEstadoRecursoRequest request)
        {
            int usuarioEjecutorId = ObtenerUsuarioId();

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
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
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
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
        public async Task<IActionResult> ConsultarTodos()
        {
            var recursos = await _gestionCatalogo.ConsultarTodosAsync();

            return Ok(recursos);
        }

        // GET: api/catalogo/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
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
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor")]
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarRecurso(
            int id,
            [FromQuery] string? motivo)
        {
            int usuarioEjecutorId = ObtenerUsuarioId();

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