using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarHistorialRecurso
    {
        private readonly IRepositorioRecurso _recursos;

        public ConsultarHistorialRecurso(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>> EjecutarAsync(
            ConsultarHistorialRecursoRequest request)
        {
            if (request.RecursoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>.Error(
                    "El recurso es obligatorio."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>.Error(
                    "El recurso no existe."
                );
            }

            var historial = new List<HistorialRecursoResponse>();

            return ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>.Ok(
                "La consulta de historial del recurso queda preparada. La carga real del historial se conectará al módulo de auditoría o persistencia.",
                historial
            );
        }
    }
}