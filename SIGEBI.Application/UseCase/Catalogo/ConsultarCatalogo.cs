using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarCatalogo
    {
        private readonly IRepositorioRecurso _recursos;

        public ConsultarCatalogo(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<IEnumerable<RecursoResponse>> EjecutarAsync(
            ConsultarCatalogoRequest request)
        {
            var recursos = await _recursos.ConsultarCatalogoAsync(
                request.Titulo,
                request.Autor,
                request.Categoria,
                request.SoloDisponibles
            );

            return recursos.Select(MapearRecurso).ToList();
        }

        public async Task<IEnumerable<RecursoResponse>> ConsultarTodosAsync()
        {
            var recursos = await _recursos.ObtenerTodosAsync();

            return recursos.Select(MapearRecurso).ToList();
        }

        private static RecursoResponse MapearRecurso(RecursoBibliografico recurso)
        {
            return new RecursoResponse
            {
                RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                ISBN = recurso.ISBN,
                Titulo = recurso.Titulo,
                Autor = recurso.Autor,
                CategoriaId = recurso.CategoriaId,
                Categoria = recurso.Categoria?.Nombre ?? "N/A",
                AnioPublicado = recurso.AnioPublicado,
                ImagenUrl = recurso.ImagenUrl,
                TotalEjemplares = recurso.TotalEjemplares,
                CopiasDisponibles = recurso.CopiasDisponibles
            };
        }
    }
}