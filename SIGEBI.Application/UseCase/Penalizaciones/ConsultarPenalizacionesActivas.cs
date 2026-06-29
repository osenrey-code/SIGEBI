using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Penalizaciones
{
    public class ConsultarPenalizacionesActivas
    {
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;

        public ConsultarPenalizacionesActivas(
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>> EjecutarAsync(
            ConsultarPenalizacionesActivasRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Bibliotecario &&
                usuarioEjecutor.Tipo != TipoUsuario.Administrador &&
                usuarioEjecutor.Tipo != TipoUsuario.Auditor)
            {
                return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                    "Solo bibliotecario, administrador o auditor pueden consultar penalizaciones activas."
                );
            }

            Guid? perfilLectorId = request.PerfilLectorId;

            if (!perfilLectorId.HasValue && request.UsuarioId.HasValue)
            {
                var usuarioConsultado = await _usuarios.ObtenerConPerfilAsync(
                    request.UsuarioId.Value
                );

                if (usuarioConsultado is null)
                {
                    return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                        "El usuario consultado no existe."
                    );
                }

                if (usuarioConsultado.PerfilLector is null)
                {
                    return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                        "El usuario consultado no tiene perfil lector."
                    );
                }

                perfilLectorId = usuarioConsultado.PerfilLector.Id;
            }

            if (!perfilLectorId.HasValue || perfilLectorId.Value == Guid.Empty)
            {
                return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                    "Debe indicar un usuario o un perfil lector para consultar penalizaciones activas."
                );
            }

            var penalizacionActiva = await _penalizaciones.ObtenerActivaPorPerfilLectorAsync(
                perfilLectorId.Value
            );

            var response = new List<PenalizacionResponse>();

            if (penalizacionActiva is not null)
            {
                response.Add(MapearPenalizacion(penalizacionActiva));
            }

            return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Ok(
                "Consulta de penalizaciones activas realizada correctamente.",
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