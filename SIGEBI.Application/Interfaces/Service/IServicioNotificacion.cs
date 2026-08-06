using SIGEBI.Application.DTOs.Response;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IServicioNotificacion
    {
        Task<IEnumerable<NotificacionResponse>> ObtenerPendientesAsync(int usuarioId);
        Task<IEnumerable<NotificacionResponse>> ObtenerTodasAsync(int usuarioId);
        Task MarcarComoLeidaAsync(int notificacionId);
        Task EnviarNotificacionAsync(int usuarioId, string mensaje, TipoNotificacion tipo);
        Task GenerarNotificacionesDeVencimientoAsync(int diasAntelacion);
        Task<IEnumerable<NotificacionResponse>> ConsultarHistorialGlobalAsync();
        Task EliminarAsync(int notificacionId);
    }
}