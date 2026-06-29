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

        public async Task<ResultadoOperacionResponse<IEnumerable<RecursoResponse>>> EjecutarAsync(
            ConsultarCatalogoRequest request)
        {
            request ??= new ConsultarCatalogoRequest();

            var recursos = await _recursos.ConsultarCatalogoAsync(
                request.Titulo,
                request.Autor,
                request.Categoria,
                request.SoloDisponibles
            );

            var response = recursos
                .Select(MapearRecurso)
                .ToList();

            return ResultadoOperacionResponse<IEnumerable<RecursoResponse>>.Ok(
                "Consulta del catálogo realizada correctamente.",
                response
            );
        }

        private static RecursoResponse MapearRecurso(RecursoBibliografico recurso)
        {
            return new RecursoResponse
            {
                Id = recurso.Id,
                Identificador = recurso.Identificador,
                Titulo = recurso.Titulo,
                Autor = recurso.Autor,
                Categoria = recurso.Categoria,
                Estado = recurso.Estado.ToString(),
                NumeroEjemplares = recurso.NumeroEjemplares
            };
        }
    }
}