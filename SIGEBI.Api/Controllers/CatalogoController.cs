using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Api.Models;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

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
        public async Task<IActionResult> RegistrarRecurso([FromForm] RegistrarRecursoFormRequest apiRequest)
        {
            string? imagenUrl = null;

            // Procesar el archivo recibido en la API
            if (apiRequest.ImagenArchivo != null && apiRequest.ImagenArchivo.Length > 0)
            {
                using var stream = apiRequest.ImagenArchivo.OpenReadStream();
                var extension = Path.GetExtension(apiRequest.ImagenArchivo.FileName);
                imagenUrl = await _storageService.GuardarAsync(stream, extension, "imagenes");
            }

            // Mapeo hacia el DTO limpio de Application
            var appRequest = new RegistrarRecursoRequest
            {
                ISBN = apiRequest.ISBN,
                Titulo = apiRequest.Titulo,
                Autor = apiRequest.Autor,
                CategoriaId = apiRequest.CategoriaId,
                AnioPublicado = apiRequest.AnioPublicado,
                CantidadEjemplares = apiRequest.CantidadEjemplares,
                ImagenUrl = imagenUrl
            };

            int usuarioEjecutorId = ObtenerUsuarioId();

            var recurso = await _gestionCatalogo.RegistrarRecursoAsync(
                appRequest,
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
        public async Task<IActionResult> ActualizarRecurso([FromForm] ActualizarRecursoApiRequest apiRequest)
        {
            string? imagenUrl = apiRequest.ImagenUrlActual;

            // Si se envió una nueva imagen desde la API
            if (apiRequest.NuevaImagenArchivo != null && apiRequest.NuevaImagenArchivo.Length > 0)
            {
                using var stream = apiRequest.NuevaImagenArchivo.OpenReadStream();
                var extension = Path.GetExtension(apiRequest.NuevaImagenArchivo.FileName);
                imagenUrl = await _storageService.GuardarAsync(stream, extension, "imagenes");
            }

            // Mapeo hacia el DTO limpio de Application
            var appRequest = new ActualizarRecursoRequest
            {
                RecursoBibliograficoId = apiRequest.RecursoBibliograficoId,
                Titulo = apiRequest.Titulo,
                Autor = apiRequest.Autor,
                CategoriaId = apiRequest.CategoriaId,
                AnioPublicado = apiRequest.AnioPublicado,
                CantidadEjemplares = apiRequest.CantidadEjemplares,
                ImagenUrl = imagenUrl
            };

            int usuarioEjecutorId = ObtenerUsuarioId();

            var recurso = await _gestionCatalogo.ActualizarRecursoAsync(
                appRequest,
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