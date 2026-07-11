using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Penalizaciones
{
    public class GestionPenalizaciones : IGestionPenalizaciones
    {
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioNotificacion _notificaciones;

        public GestionPenalizaciones(
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            IAuditoriaService auditoria,
            IServicioNotificacion notificaciones)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _auditoria = auditoria;
            _notificaciones = notificaciones;
        }

        public async Task<IEnumerable<PenalizacionResponse>> ConsultarPenalizacionesAsync(
            ConsultarPenalizacionesRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los filtros de penalizaciones");

            await ValidarUsuarioAutorizadoParaConsultaAsync(
                usuarioId,
                "Solo personal autorizado puede consultar penalizaciones."
            );

            EstadoPenalizacion? estado = null;

            if (!string.IsNullOrWhiteSpace(request.Estado))
            {
                if (!Enum.TryParse<EstadoPenalizacion>(
                        request.Estado.Trim(),
                        true,
                        out var estadoConvertido))
                {
                    throw new BusinessException("El estado de penalización no es válido.");
                }

                estado = estadoConvertido;
            }

            var penalizaciones = await _penalizaciones.ConsultarAsync(
                request.UsuarioId,
                request.PrestamoId,
                estado,
                null,
                null
            );

            return penalizaciones
                .Select(MapearPenalizacion)
                .ToList();
        }

        public async Task<IEnumerable<PenalizacionResponse>> ConsultarPenalizacionesActivasAsync(
            ConsultarPenalizacionesActivasRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los filtros de penalizaciones activas");

            if (request.UsuarioId <= 0)
                throw new BusinessException("El usuario consultado es obligatorio.");

            await ValidarUsuarioAutorizadoParaConsultaAsync(
                usuarioId,
                "Solo personal autorizado puede consultar penalizaciones activas."
            );

            var penalizaciones = await _penalizaciones.ConsultarAsync(
                request.UsuarioId,
                null,
                EstadoPenalizacion.Activa,
                null,
                null
            );

            return penalizaciones
                .Select(MapearPenalizacion)
                .ToList();
        }

        public async Task<PenalizacionResponse> ResolverPenalizacionAsync(
            ResolverPenalizacionRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los datos de resolución de penalización");

            if (request.PenalizacionId <= 0)
                throw new BusinessException("La penalización es obligatoria.");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario responsable de la resolución es obligatorio.");

            Guard.NotNullOrWhiteSpace(
                request.MotivoResolucion,
                "El motivo de resolución"
            );

            string motivoResolucion = request.MotivoResolucion.Trim();

            await ValidarUsuarioAutorizadoParaResolverAsync(
                usuarioId
            );

            var penalizacion = await _penalizaciones.ObtenerporIdAsync(
                request.PenalizacionId
            );

            if (penalizacion is null)
                throw new BusinessException("La penalización no existe.");

            penalizacion.Resolver(
                usuarioId,
                motivoResolucion
            );

            await _penalizaciones.ActualizarAsync(
                penalizacion
            );

            await _notificaciones.EnviarNotificacionAsync(
                penalizacion.UsuarioId,
                $"Tu penalización #{penalizacion.PenalizacionId} fue marcada como resuelta.",
                TipoNotificacion.PenalizacionResuelta
            );

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Resolver Penalización",
                EntidadAfectada: "Penalizaciones",
                detalles: $"Se resolvió la penalización ID {penalizacion.PenalizacionId} del usuario ID {penalizacion.UsuarioId}. Motivo: {motivoResolucion}."
            );

            return MapearPenalizacion(
                penalizacion
            );
        }

        private async Task<Usuario> ValidarUsuarioAutorizadoParaConsultaAsync(
            int usuarioId,
            string mensajeNoAutorizado)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(
                usuarioId
            );

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario &&
                usuarioEjecutor is not Administrador &&
                usuarioEjecutor is not Auditor)
            {
                throw new BusinessException(mensajeNoAutorizado);
            }

            return usuarioEjecutor;
        }

        private async Task<Usuario> ValidarUsuarioAutorizadoParaResolverAsync(
            int usuarioId)
        {
            var usuarioResponsable = await _usuarios.ObtenerporIdAsync(
                usuarioId
            );

            if (usuarioResponsable is null)
                throw new BusinessException("El usuario responsable no existe.");

            if (usuarioResponsable.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario responsable no está activo.");

            if (usuarioResponsable is not Bibliotecario &&
                usuarioResponsable is not Administrador)
            {
                throw new BusinessException("Solo un bibliotecario o administrador puede resolver penalizaciones.");
            }

            return usuarioResponsable;
        }

        private static PenalizacionResponse MapearPenalizacion(
            Penalizacion penalizacion)
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