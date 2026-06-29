using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarDetalleRecurso
    {
        private readonly IRepositorioRecurso _recursos;

        public ConsultarDetalleRecurso(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<ResultadoOperacionResponse<RecursoResponse>> EjecutarAsync(
            ConsultarDetalleRecursoRequest request)
        {
            if (request.RecursoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El recurso es obligatorio."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<RecursoResponse>.Error(
                    "El recurso no existe."
                );
            }

            var response = MapearRecurso(recurso);

            return ResultadoOperacionResponse<RecursoResponse>.Ok(
                "Detalle del recurso consultado correctamente.",
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