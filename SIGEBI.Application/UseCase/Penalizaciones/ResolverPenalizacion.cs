using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Penalizaciones
{
    public class ResolverPenalizacion
    {
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;

        public ResolverPenalizacion(
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<PenalizacionResponse>> EjecutarAsync(
            ResolverPenalizacionRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            if (request.PenalizacionId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "La penalización es obligatoria."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Bibliotecario &&
                usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "Solo un bibliotecario o administrador puede resolver penalizaciones."
                );
            }

            var penalizacion = await _penalizaciones.ObtenerPorIdAsync(
                request.PenalizacionId
            );

            if (penalizacion is null)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "La penalización no existe."
                );
            }

            if (penalizacion.Estado != EstadoPenalizacion.Activa)
            {
                return ResultadoOperacionResponse<PenalizacionResponse>.Error(
                    "Solo se pueden resolver penalizaciones activas."
                );
            }

            penalizacion.Resolver(request.UsuarioEjecutorId);

            await _penalizaciones.ActualizarAsync(penalizacion);

            var response = MapearPenalizacion(penalizacion);

            return ResultadoOperacionResponse<PenalizacionResponse>.Ok(
                "Penalización resuelta correctamente.",
                response
            );
        }

        private static PenalizacionResponse MapearPenalizacion(Penalizacion penalizacion)
        {
            return new PenalizacionResponse
            {
                Id = penalizacion.Id,
                PerfilLectorId = penalizacion.PerfilLectorId,
                UsuarioId = penalizacion.PerfilLector?.UsuarioId,
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