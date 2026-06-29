using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Penalizaciones
{
    public class ConsultarPenalizaciones
    {
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;

        public ConsultarPenalizaciones(
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>> EjecutarAsync(
            ConsultarPenalizacionesRequest request)
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
                    "Solo bibliotecario, administrador o auditor pueden consultar penalizaciones."
                );
            }

            EstadoPenalizacion? estado = null;

            if (!string.IsNullOrWhiteSpace(request.Estado))
            {
                if (!Enum.TryParse<EstadoPenalizacion>(
                        request.Estado,
                        ignoreCase: true,
                        out var estadoParseado))
                {
                    return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                        "El estado indicado no es válido. Estados permitidos: Activa, Resuelta."
                    );
                }

                estado = estadoParseado;
            }

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var penalizaciones = await _penalizaciones.ConsultarAsync(
                request.UsuarioId,
                request.PerfilLectorId,
                estado,
                request.FechaInicio,
                request.FechaFin
            );

            var response = penalizaciones
                .Select(MapearPenalizacion)
                .ToList();

            return ResultadoOperacionResponse<IEnumerable<PenalizacionResponse>>.Ok(
                "Consulta de penalizaciones realizada correctamente.",
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