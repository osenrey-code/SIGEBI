using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly IGestionCategorias _gestionCategorias;

        public CategoriasController(IGestionCategorias gestionCategorias)
        {
            _gestionCategorias = gestionCategorias;
        }

        // POST: api/categorias/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCategoria(
            [FromBody] CategoriaRequest request)
        {
            int actorId = 1;

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
        public async Task<IActionResult> ConsultarCategorias()
        {
            var categorias = await _gestionCategorias
                .ConsultarCategoriasAsync();

            return Ok(categorias);
        }
    }
}