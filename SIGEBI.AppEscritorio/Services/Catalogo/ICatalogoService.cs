using SIGEBI.AppEscritorio.Dtos.Catalogo.Request;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Response;

namespace SIGEBI.AppEscritorio.Services.Interfaces
{
    public interface ICatalogoService
    {
        Task<IEnumerable<RecursoResponse>?> ConsultarTodosAsync();
        Task<IEnumerable<RecursoResponse>?> ConsultarCatalogoAsync(ConsultarCatalogoRequest request);
        Task<RecursoResponse?> ConsultarDetalleAsync(int id);
        Task<IEnumerable<HistorialRecursoResponse>?> ConsultarHistorialAsync(int id);

        Task RegistrarRecursoAsync(RegistrarRecursoRequest request);
        Task ActualizarRecursoAsync(ActualizarRecursoRequest request);

        Task CambiarEstadoRecursoAsync(CambiarEstadoRecursoRequest request);
        Task EliminarRecursoAsync(int id, string? motivo);
    }
}