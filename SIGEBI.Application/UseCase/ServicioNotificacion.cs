using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase
{
    public class ServicioNotificacion : IServicioNotificacion
    {
        private readonly IRepositorioNotificacion _repoNotificacion;
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IApplicationDbContext _db;

        public ServicioNotificacion(
            IRepositorioNotificacion repoNotificacion,
            IRepositorioPrestamo repoPrestamo,
            IApplicationDbContext db)
        {
            _repoNotificacion = repoNotificacion;
            _repoPrestamo = repoPrestamo;
            _db = db;
        }

        // 🟢 Obtiene sólo las no leídas
        public async Task<IEnumerable<NotificacionResponse>> ObtenerPendientesAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario es obligatorio.");

            var pendientes = await _repoNotificacion.ObtenerNoLeidasPorUsuarioAsync(usuarioId);

            return pendientes
                .Select(MapearNotificacion)
                .ToList();
        }

        // 🟢 NUEVO: Obtiene todas las notificaciones del usuario (Leídas y No Leídas)
        public async Task<IEnumerable<NotificacionResponse>> ObtenerTodasAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario es obligatorio.");

            var todas = await _repoNotificacion.ObtenerPorUsuarioAsync(usuarioId);

            return todas
                .Select(MapearNotificacion)
                .ToList();
        }

        public async Task MarcarComoLeidaAsync(int notificacionId)
        {
            if (notificacionId <= 0)
                throw new BusinessException("La notificación es obligatoria.");

            var notificacion = await _repoNotificacion.ObtenerporIdAsync(notificacionId);

            if (notificacion is null)
                throw new BusinessException("La notificación no existe.");

            if (notificacion.Leida)
                return;

            notificacion.MarcarComoLeida();

            await _repoNotificacion.ActualizarAsync(notificacion);
            await _db.SaveChangesAsync();
        }

        public async Task EnviarNotificacionAsync(
            int usuarioId,
            string mensaje,
            TipoNotificacion tipo)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario destinatario es obligatorio.");

            if (string.IsNullOrWhiteSpace(mensaje))
                throw new BusinessException("El mensaje de la notificación es obligatorio.");

            var notificacion = new Notificacion(
                usuarioId,
                tipo,
                mensaje.Trim()
            );

            await _repoNotificacion.AgregarAsync(notificacion);
            await _db.SaveChangesAsync();
        }

        public async Task GenerarNotificacionesDeVencimientoAsync(int diasAntelacion)
        {
            if (diasAntelacion < 0)
                throw new BusinessException("Los días de antelación no pueden ser negativos.");

            DateTime fechaObjetivo = DateTime.UtcNow.Date.AddDays(diasAntelacion);

            var prestamosActivos = await _repoPrestamo.ConsultarActivosAsync(null, null, null);

            var prestamosAVencer = prestamosActivos
                .Where(p => p.FechaLimite.Date == fechaObjetivo)
                .ToList();

            foreach (var prestamo in prestamosAVencer)
            {
                string mensaje =
                    $"Recordatorio: tu préstamo #{prestamo.PrestamoId} vence el {prestamo.FechaLimite:dd/MM/yyyy}.";

                await EnviarNotificacionAsync(
                    prestamo.UsuarioId,
                    mensaje,
                    TipoNotificacion.RecordatorioVencimiento
                );
            }
        }

        public async Task EliminarAsync(int notificacionId)
        {
            if (notificacionId <= 0)
                throw new BusinessException("La notificación es obligatoria.");

            var notificacion = await _repoNotificacion.ObtenerporIdAsync(notificacionId);

            if (notificacion is null)
                throw new BusinessException("La notificación no existe.");

            await _repoNotificacion.EliminarAsync(notificacion);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificacionResponse>> ConsultarHistorialGlobalAsync()
        {
            var historial = await _repoNotificacion.ObtenerTodoElHistorialAsync();

            return historial
                .Select(MapearNotificacion)
                .ToList();
        }

        private static NotificacionResponse MapearNotificacion(Notificacion notificacion)
        {
            return new NotificacionResponse
            {
                NotificacionId = notificacion.NotificacionId,
                UsuarioId = notificacion.UsuarioId,
                Tipo = notificacion.Tipo.ToString(),
                Mensaje = notificacion.Mensaje,
                FechaRegistro = notificacion.FechaRegistro,
                Leida = notificacion.Leida
            };
        }
    }
}