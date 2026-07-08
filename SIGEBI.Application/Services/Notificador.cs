using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.Services
{
    public class Notificador : INotificador
    {
        private readonly IRepositorioNotificacion _notificaciones;

        public Notificador(IRepositorioNotificacion notificaciones)
        {
            _notificaciones = notificaciones;
        }

        public async Task NotificarAsync(
            int usuarioId,
            TipoNotificacion tipo,
            string mensaje)
        {
            var existe = await _notificaciones.ExisteAsync(
                usuarioId,
                tipo,
                mensaje
            );

            if (existe)
                return;

            var notificacion = new Notificacion(
                usuarioId,
                tipo,
                mensaje
            );

            await _notificaciones.AgregarAsync(notificacion);
        }

        public async Task NotificarPenalizacionGeneradaAsync(
            int usuarioId,
            int penalizacionId)
        {
            await NotificarAsync(
                usuarioId,
                TipoNotificacion.PenalizacionGenerada,
                $"Se ha generado una penalización con ID {penalizacionId}."
            );
        }

        public async Task NotificarPenalizacionResueltaAsync(
            int usuarioId,
            int penalizacionId)
        {
            await NotificarAsync(
                usuarioId,
                TipoNotificacion.PenalizacionResuelta,
                $"La penalización con ID {penalizacionId} ha sido resuelta."
            );
        }

        public async Task NotificarPrestamoFormalizadoAsync(
            int usuarioId,
            int prestamoId)
        {
            await NotificarAsync(
                usuarioId,
                TipoNotificacion.PrestamoFormalizado,
                $"El préstamo con ID {prestamoId} ha sido formalizado."
            );
        }

        public async Task NotificarSolicitudRecibidaAsync(
            int usuarioId,
            int solicitudId)
        {
            await NotificarAsync(
                usuarioId,
                TipoNotificacion.SolicitudRecibida,
                $"Se ha recibido la solicitud con ID {solicitudId}."
            );
        }

        public async Task EnviarRecordatorioVencimientoAsync(
            int usuarioId,
            int prestamoId,
            DateTime fechaLimite)
        {
            await NotificarAsync(
                usuarioId,
                TipoNotificacion.RecordatorioVencimiento,
                $"El préstamo con ID {prestamoId} vence el {fechaLimite:dd/MM/yyyy}."
            );
        }
    }
}
