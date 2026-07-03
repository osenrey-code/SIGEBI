using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

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

        public async Task<IEnumerable<PenalizacionResponse>> EjecutarAsync(
            ConsultarPenalizacionesActivasRequest request)
        {
            if (request.UsuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.UsuarioId <= 0)
                throw new BusinessException("El usuario consultado es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(request.UsuarioEjecutorId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario &&
                usuarioEjecutor is not Administrador &&
                usuarioEjecutor is not Auditor)
            {
                throw new BusinessException("Solo personal autorizado puede consultar penalizaciones activas.");
            }

            var penalizaciones = await _penalizaciones.ObtenerActivasPorUsuarioAsync(
                request.UsuarioId
            );

            return penalizaciones
                .Select(MapearPenalizacion)
                .ToList();
        }

        private static PenalizacionResponse MapearPenalizacion(Penalizacion penalizacion)
        {
            return new PenalizacionResponse
            {
                IdPenalizacion = penalizacion.IdPenalizacion,
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