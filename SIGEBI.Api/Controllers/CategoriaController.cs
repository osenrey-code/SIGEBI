using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    [Authorize]
    public class CategoriasController : BaseApiController
    {
        private readonly IGestionCategorias _gestionCategorias;

        public CategoriasController(IGestionCategorias gestionCategorias)
        {
            _gestionCategorias = gestionCategorias;
        }

        // POST: api/categorias/registrar
        [HttpPost("registrar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistrarCategoria(
            [FromBody] CategoriaRequest request)
        {
            int actorId = ObtenerUsuarioId();

            var categoria = await _gestionCategorias
                .RegistrarCategoriaAsync(request, actorId);

            return Ok(new
            {
                Mensaje = "Categoría registrada correctamente.",
                Categoria = categoria
            });
        }

        // GET: api/categorias
        [HttpGet]
        [Authorize(Roles = "Administrador,Bibliotecario,Auditor,Estudiante,Docente")]
        public async Task<IActionResult> ConsultarCategorias()
        {
            var categorias = await _gestionCategorias
                .ConsultarCategoriasAsync();

            return Ok(categorias);
        }
    }
}