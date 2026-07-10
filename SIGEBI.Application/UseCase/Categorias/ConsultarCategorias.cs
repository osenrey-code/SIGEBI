using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarCategorias
    {
        private readonly IRepositorioCategoria _categorias;

        public ConsultarCategorias(IRepositorioCategoria categorias)
        {
            _categorias = categorias;
        }

        public async Task<IEnumerable<CategoriaResponse>> ConsultarCategoriasAsync()
        {
            var categorias = await _categorias.ObtenerTodosAsync();

            return categorias
                .OrderBy(c => c.Nombre)
                .Select(c => new CategoriaResponse
                {
                    CategoriaId = c.CategoriaId,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion
                })
                .ToList();
        }
    }
}