using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Exceptions;

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
            if (request is null)
                throw new BusinessException("Los filtros de consulta son obligatorios.");

            string? titulo = string.IsNullOrWhiteSpace(request.Titulo)
                ? null
                : request.Titulo.Trim();

            string? autor = string.IsNullOrWhiteSpace(request.Autor)
                ? null
                : request.Autor.Trim();

            string? categoria = string.IsNullOrWhiteSpace(request.Categoria)
                ? null
                : request.Categoria.Trim();

            var recursos = await _recursos.ConsultarCatalogoAsync(
                titulo,
                autor,
                categoria,
                request.SoloDisponibles
            );

            return recursos
                .Select(MapearRecurso)
                .ToList();
        }

        public async Task<IEnumerable<RecursoResponse>> ConsultarTodosAsync()
        {
            var recursos = await _recursos.ObtenerTodosAsync();

            return recursos
                .Select(MapearRecurso)
                .ToList();
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