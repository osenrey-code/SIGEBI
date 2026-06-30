using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using System.Net;

namespace SIGEBI.Application.UseCase.Penalizaciones
{
    public class ResolverPenalizacion
    {
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly IRepositorioPerfilLector _perfilLector;
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;

        public ResolverPenalizacion(IRepositorioPenalizacion penalizaciones, IUsuario usuarios,
            IRepositorioPerfilLector perfilLector,
            INotificador notificador, IAuditoriaService auditoria)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _perfilLector = perfilLector;
            _notificador = notificador;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<PenalizacionResponse>> EjecutarAsync(
            ResolverPenalizacionRequest request)
        {
            // Validamos que venga el usuario que está ejecutando la acción.
            // Este usuario debe ser quien resuelve la penalización en el sistema.
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            // Validamos que venga la penalización que se quiere resolver.
            if (request.PenalizacionId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "La penalización es obligatoria."
                );
            }

            // Buscamos al usuario ejecutor para verificar si existe y si tiene permiso.
            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            // El usuario responsable debe estar activo para poder operar.
            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            // Solo Bibliotecario o Administrador pueden resolver penalizaciones.
            if (usuarioEjecutor.Tipo != TipoUsuario.Bibliotecario &&
                usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "Solo un bibliotecario o administrador puede resolver penalizaciones."
                );
            }

            // Buscamos la penalización por su Id.
            var penalizacion = await _penalizaciones.ObtenerPorIdAsync(
                request.PenalizacionId
            );

            if (penalizacion is null)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "La penalización no existe."
                );
            }

            // Solo una penalización activa puede resolverse.
            if (penalizacion.Estado != EstadoPenalizacion.Activa)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "Solo se pueden resolver penalizaciones activas."
                );
            }

            // La penalización pertenece a un PerfilLector.
            // Necesitamos ese perfil para saber cuál UsuarioId debe recibir la notificación.
            var perfilLector = await _perfilLector.ObtenerPorIdAsync(
                penalizacion.PerfilLectorId
            );

            if (perfilLector is null)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El perfil lector asociado a la penalización no existe."
                );
            }

            try
            {
                
                penalizacion.Resolver(request.UsuarioEjecutorId);
            }
            catch (BusinessException ex)
            {
                
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    ex.Message
                );
            }

            // Guardamos el cambio de estado de la penalización.
            await _penalizaciones.ActualizarAsync(penalizacion);

            // Después de guardar, notificamos al estudiante/docente afectado.
            await _notificador.NotificarPenalizacionResueltaAsync(
                perfilLector.UsuarioId,
                penalizacion.Id
            );

            //Registrar Auditoria
            await _auditoria.RegistrarAsync(
            request.UsuarioEjecutorId,
            "Resolver penalización",
            "Penalizacion",
            penalizacion.Id,
            "Exitoso",
            $"La penalización {penalizacion.Id} fue resuelta correctamente. " +
            $"Usuario afectado: {perfilLector.UsuarioId}."
            );

            // Convertimos la entidad a DTO de respuesta.
            var response = MapearPenalizacion(
                penalizacion,
                perfilLector.UsuarioId
            );

            return ResultadoOperacionResponse<PenalizacionResponse>.Ok(
                "Penalización resuelta correctamente.",
                response
            );
        }

        private static PenalizacionResponse MapearPenalizacion(
            Penalizacion penalizacion,
            Guid usuarioId)
        {
            return new PenalizacionResponse
            {
                Id = penalizacion.Id,
                PerfilLectorId = penalizacion.PerfilLectorId,

  
                UsuarioId = usuarioId,
                Motivo = CrearMotivo(penalizacion),

                Estado = penalizacion.Estado.ToString(),
                FechaGeneracion = penalizacion.FechaGeneracion,
                FechaResolucion = penalizacion.FechaResolucion,
                UsuarioResolucionId = penalizacion.UsuarioResolucionId
            };
        }

        private static string CrearMotivo(Penalizacion penalizacion)
        {
            return $"Devolución tardía de {penalizacion.DiasRetraso} día(s). Monto de mora: {penalizacion.MontoMora}.";
        }
    }
}