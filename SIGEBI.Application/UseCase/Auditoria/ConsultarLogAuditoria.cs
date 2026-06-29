using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Auditoria
{
    public class ConsultarLogAuditoria
    {
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IUsuario _usuarios;

        public ConsultarLogAuditoria(
            IRepositorioAuditoria auditoria,
            IUsuario usuarios)
        {
            _auditoria = auditoria;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>> EjecutarAsync(
            ConsultarLogAuditoriaRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador &&
                usuarioEjecutor.Tipo != TipoUsuario.Auditor)
            {
                return ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>.Error(
                    "Solo un administrador o auditor puede consultar los registros de auditoría."
                );
            }

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                return ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var registros = await _auditoria.ConsultarAsync(
                request.UsuarioId,
                request.Accion,
                request.EntidadAfectada,
                request.FechaInicio,
                request.FechaFin
            );

            var response = registros
                .Select(MapearRegistro)
                .ToList();

            return ResultadoOperacionResponse<IEnumerable<LogAuditoriaResponse>>.Ok(
                "Consulta de auditoría realizada correctamente.",
                response
            );
        }

        private static LogAuditoriaResponse MapearRegistro(RegistroAuditoria registro)
        {
            return new LogAuditoriaResponse
            {
                Id = registro.Id,
                UsuarioId = registro.UsuarioId,
                Usuario = registro.Usuario,
                Accion = registro.Accion,
                EntidadAfectada = registro.EntidadAfectada,
                EntidadId = registro.EntidadId,
                Resultado = registro.Resultado,
                Detalle = registro.Detalle,
                FechaRegistro = registro.FechaRegistro,
                ValoresAnteriores = registro.ValoresAnteriores,
                ValoresNuevos = registro.ValoresNuevos
            };
        }
    }
}