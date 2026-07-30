using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IGestionCatalogo
    {
        Task<RecursoResponse> RegistrarRecursoAsync(
            RegistrarRecursoRequest request,
            int usuarioEjecutorId
        );

        Task<RecursoResponse> ActualizarRecursoAsync(
            ActualizarRecursoRequest request,
            int usuarioId
        );

        Task<RecursoResponse> CambiarEstadoRecursoAsync(
            CambiarEstadoRecursoRequest request,
            int usuarioId
        );

        Task<IEnumerable<RecursoResponse>> ConsultarCatalogoAsync(
            ConsultarCatalogoRequest request
        );

        Task<IEnumerable<RecursoResponse>> ConsultarTodosAsync();

        Task<RecursoResponse> ConsultarDetalleRecursoAsync(
            ConsultarDetalleRecursoRequest request
        );

        Task<IEnumerable<HistorialRecursoResponse>> ConsultarHistorialRecursoAsync(
            ConsultarHistorialRecursoRequest request
        );

        Task EliminarRecursoAsync(
            EliminarRecursoRequest request,
            int usuarioId
        );

        Task<int?> ObtenerPrimerEjemplarDisponibleIdAsync(int recursoId);
    }
}