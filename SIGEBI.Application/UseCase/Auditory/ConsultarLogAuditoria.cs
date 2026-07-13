using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Auditory
{
    public class ConsultarLogAuditoria
    {
        private readonly IRepositorioAuditoria _auditoria;
        private readonly IUsuario _usuarios;

        public ConsultarLogAuditoria(IRepositorioAuditoria auditoria, IUsuario usuarios)
        {
            _auditoria = auditoria;
            _usuarios = usuarios;
        }

        public async Task<IEnumerable<LogAuditoriaResponse>> EjecutarAsync(
            ConsultarLogAuditoriaRequest request, int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Administrador && usuarioEjecutor is not Auditor)
                throw new BusinessException("Solo un administrador o auditor puede consultar los registros de auditoría.");

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                throw new BusinessException("La fecha de inicio no puede ser mayor que la fecha final.");
            }

            var registros = await _auditoria.ConsultarAsync(
                request.UsuarioId,
                request.Accion,
                request.EntidadAfectada,
                request.FechaInicio,
                request.FechaFin
            );

            return registros.Select(MapearRegistro).ToList();
        }

        private static LogAuditoriaResponse MapearRegistro(SIGEBI.Domain.Entities.Auditoria registro)
        {
            return new LogAuditoriaResponse
            {
                AuditoriaId = registro.AuditoriaId,
                UsuarioId = registro.UsuarioId,
                Accion = registro.Accion,
                EntidadAfectada = registro.EntidadAfectada,
                Detalle = registro.Detalle,
                FechaRegistro = registro.FechaRegistro
            };
        }
    }
}