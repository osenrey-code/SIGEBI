using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

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

        public async Task<IEnumerable<PenalizacionResponse>> EjecutarAsync(
            ConsultarPenalizacionesRequest request, int usuarioId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario &&
                usuarioEjecutor is not Administrador &&
                usuarioEjecutor is not Auditor)
            {
                throw new BusinessException("Solo personal autorizado puede consultar penalizaciones.");
            }

            EstadoPenalizacion? estado = null;

            if (!string.IsNullOrWhiteSpace(request.Estado))
            {
                if (!Enum.TryParse<EstadoPenalizacion>(
                        request.Estado,
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