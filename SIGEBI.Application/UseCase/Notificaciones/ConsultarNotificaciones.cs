using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Notificaciones
{
    public class ConsultarNotificaciones
    {
        private readonly IRepositorioNotificacion _notificaciones;
        private readonly IUsuario _usuarios;

        public ConsultarNotificaciones(
            IRepositorioNotificacion notificaciones,
            IUsuario usuarios)
        {
            _notificaciones = notificaciones;
            _usuarios = usuarios;
        }

        public async Task<IEnumerable<NotificacionResponse>> EjecutarAsync(
            ConsultarNotificacionesRequest request, int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (request.UsuarioId.HasValue &&
                request.UsuarioId.Value != usuarioId &&
                usuarioEjecutor is not Bibliotecario &&
                usuarioEjecutor is not Administrador &&
                usuarioEjecutor is not Auditor)
            {
                throw new BusinessException("Solo personal autorizado puede consultar notificaciones de otros usuarios.");
            }

            var notificaciones = await _notificaciones.ConsultarAsync(
                request.UsuarioId,
                request.Tipo
            );

            return notificaciones
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