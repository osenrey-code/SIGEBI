using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
namespace SIGEBI.Application.Services
{
    public class Notificador : INotificador
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IRepositorioNotificacion _notificaciones;
        private readonly IServicioCorreo _servicioCorreo;

        public Notificador(
            IUsuario usuarios,
            IRepositorioPenalizacion penalizaciones,
            IRepositorioNotificacion notificaciones,
            IServicioCorreo servicioCorreo)
        {
            _usuarios = usuarios;
            _penalizaciones = penalizaciones;
            _notificaciones = notificaciones;
            _servicioCorreo = servicioCorreo;
        }

        public async Task NotificarSolicitudPrestamoAsync(
            Guid usuarioId,
            Guid prestamoId)
        {
            var asunto = "Solicitud de préstamo recibida";

            var mensaje =
                $"Tu solicitud de préstamo fue registrada correctamente. " +
                $"Código del préstamo: {prestamoId}.";

            await EnviarYRegistrarAsync(
                usuarioId,
                "SolicitudPrestamo",
                asunto,
                mensaje
            );
        }

        public async Task NotificarPrestamoAprobadoAsync(
            Guid usuarioId,
            Guid prestamoId,
            DateTime fechaLimite)
        {
            var asunto = "Préstamo aprobado";

            var mensaje =
                $"Tu préstamo fue aprobado correctamente. " +
                $"Código del préstamo: {prestamoId}. " +
                $"Fecha límite de devolución: {fechaLimite:dd/MM/yyyy}.";

            await EnviarYRegistrarAsync(
                usuarioId,
                "PrestamoAprobado",
                asunto,
                mensaje
            );
        }

        public async Task NotificarPenalizacionGeneradaAsync(
            Guid usuarioId,
            Guid penalizacionId)
        {
            var penalizacion = await _penalizaciones.ObtenerPorIdAsync(
                penalizacionId
            );

            var asunto = "Penalización generada";

            var mensaje = penalizacion is null
                ? "Se generó una penalización asociada a tu usuario."
                : $"Se generó una penalización por devolución tardía. " +
                  $"Días de retraso: {penalizacion.DiasRetraso}. " +
                  $"Monto de mora: {penalizacion.MontoMora}.";

            await EnviarYRegistrarAsync(
                usuarioId,
                "PenalizacionGenerada",
                asunto,
                mensaje
            );
        }

        public async Task NotificarPenalizacionResueltaAsync(
            Guid usuarioId,
            Guid penalizacionId)
        {
            var asunto = "Penalización resuelta";

            var mensaje =
                $"La penalización {penalizacionId} fue resuelta correctamente. " +
                "Tu usuario queda habilitado nuevamente si no tienes otras restricciones activas.";

            await EnviarYRegistrarAsync(
                usuarioId,
                "PenalizacionResuelta",
                asunto,
                mensaje
            );
        }

        private async Task EnviarYRegistrarAsync(
            Guid usuarioId,
            string tipoEvento,
            string asunto,
            string mensaje)
        {
            if (usuarioId == Guid.Empty)
            {
                return;
            }

            var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId);

            if (usuario is null ||
                string.IsNullOrWhiteSpace(usuario.Correo))
            {
                return;
            }

            var notificacion = new Notificacion(
                usuario.Id,
                usuario.Correo,
                tipoEvento,
                mensaje
            );

            try
            {
                await _servicioCorreo.EnviarAsync(
                    usuario.Correo,
                    asunto,
                    mensaje
                );

                notificacion.MarcarComoEnviada();
            }
            catch
            {
                notificacion.MarcarComoFallida();
            }

            await _notificaciones.AgregarAsync(notificacion);
        }
    }
}
