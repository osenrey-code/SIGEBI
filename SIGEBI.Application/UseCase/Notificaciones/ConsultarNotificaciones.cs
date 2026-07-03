using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

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

        public async Task<ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>> EjecutarAsync(
            ConsultarNotificacionesRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador &&
                usuarioEjecutor.Tipo != TipoUsuario.Auditor)
            {
                return ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>.Error(
                    "Solo un administrador o auditor puede consultar notificaciones."
                );
            }

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                return ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var notificaciones = await _notificaciones.ConsultarAsync(
                request.UsuarioDestinatarioId,
                request.TipoEvento,
                request.FechaInicio,
                request.FechaFin
            );

            var response = notificaciones
                .Select(MapearNotificacion)
                .ToList();

            return ResultadoOperacionResponse<IEnumerable<NotificacionResponse>>.Ok(
                "Consulta de notificaciones realizada correctamente.",
                response
            );
        }

        private static NotificacionResponse MapearNotificacion(Notificacion notificacion)
        {
            return new NotificacionResponse
            {
                Id = notificacion.Id,
                UsuarioDestinatarioId = notificacion.UsuarioDestinatarioId,
                CorreoDestinatario = notificacion.CorreoDestinatario,
                TipoEvento = notificacion.TipoEvento,
                Mensaje = notificacion.Mensaje,
                FechaRegistro = notificacion.FechaRegistro,
                EstadoEnvio = notificacion.EstadoEnvio
            };
        }
    }
}