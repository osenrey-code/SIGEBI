namespace SIGEBI.Application.Interfaces.ext
{
    public interface INotificador
    {
        Task NotificarSolicitudPrestamoAsync(Guid usuarioId, Guid prestamoId);

        Task NotificarPrestamoAprobadoAsync(
            Guid usuarioId,
            Guid prestamoId,
            DateTime fechaLimite);

        Task NotificarPenalizacionGeneradaAsync(
            Guid usuarioId,
            Guid penalizacionId);

        Task NotificarPenalizacionResueltaAsync(
            Guid usuarioId,
            Guid penalizacionId);
    }
}