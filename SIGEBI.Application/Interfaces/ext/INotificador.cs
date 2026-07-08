using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface INotificador
    {
        Task NotificarAsync(
            int usuarioId,
            TipoNotificacion tipo,
            string mensaje
        );

        Task NotificarPenalizacionGeneradaAsync(
            int usuarioId,
            int penalizacionId
        );

        Task NotificarPenalizacionResueltaAsync(
            int usuarioId,
            int penalizacionId
        );

        Task NotificarPrestamoFormalizadoAsync(
            int usuarioId,
            int prestamoId
        );

        Task NotificarSolicitudRecibidaAsync(
            int usuarioId,
            int solicitudId
        );

        Task EnviarRecordatorioVencimientoAsync(
            int usuarioId,
            int prestamoId,
            DateTime fechaLimite
        );
    }
}