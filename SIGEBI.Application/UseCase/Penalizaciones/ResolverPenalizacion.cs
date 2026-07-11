using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Penalizaciones
{
    public class ResolverPenalizacion
    {
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioNotificacion _notificaciones;

        public ResolverPenalizacion(
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            IAuditoriaService auditoria, IServicioNotificacion notificaciones)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _auditoria = auditoria;
            _notificaciones = notificaciones;
        }

        public async Task<PenalizacionResponse> EjecutarAsync(ResolverPenalizacionRequest request, int usuarioId)
        {
            if (request.PenalizacionId <= 0)
                throw new BusinessException("La penalización es obligatoria.");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario responsable de la resolución es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.MotivoResolucion))
                throw new BusinessException("El motivo de resolución es obligatorio.");

            var usuarioResponsable = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioResponsable is null)
                throw new BusinessException("El usuario responsable no existe.");

            if (usuarioResponsable.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario responsable no está activo.");

            if (usuarioResponsable is not Bibliotecario && usuarioResponsable is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede resolver penalizaciones.");

            var penalizacion = await _penalizaciones.ObtenerporIdAsync(request.PenalizacionId);

            if (penalizacion is null)
                throw new BusinessException("La penalización no existe.");

            penalizacion.Resolver(
                usuarioId,
                request.MotivoResolucion
            );

            await _penalizaciones.ActualizarAsync(penalizacion);

            await _notificaciones.EnviarNotificacionAsync(
                penalizacion.UsuarioId,
                 $"Tu penalización #{penalizacion.PenalizacionId} fue marcada como resuelta.",
                 TipoNotificacion.PenalizacionResuelta);

            await _auditoria.RegistrarAsync(
                usuarioId,
                "Resolver penalización",
                "Penalizacion",
                $"Se resolvió la penalización ID {penalizacion.PenalizacionId} del usuario ID {penalizacion.UsuarioId}."
            );

            return MapearPenalizacion(penalizacion);
        }

        private static PenalizacionResponse MapearPenalizacion(Penalizacion penalizacion)
        {
            return new PenalizacionResponse
            {
                PenalizacionId = penalizacion.PenalizacionId,
                UsuarioId = penalizacion.UsuarioId,
                PrestamoId = penalizacion.PrestamoId,
                DiasRetraso = penalizacion.DiasRetraso,
                MontoMora = penalizacion.MontoMora,
                Motivo = penalizacion.Motivo,
                Estado = penalizacion.Estado.ToString(),
                FechaGeneracion = penalizacion.FechaGeneracion,
                FechaResolucion = penalizacion.FechaResolucion,
                UsuarioResolucionId = penalizacion.UsuarioResolucionId,
                MotivoResolucion = penalizacion.MotivoResolucion
            };
        }
    }
}