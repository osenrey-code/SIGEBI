using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
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

        public ResolverPenalizacion(
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            IAuditoriaService auditoria)
        {
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<PenalizacionResponse> EjecutarAsync(ResolverPenalizacionRequest request)
        {
            if (request.IdPenalizacion <= 0)
                throw new BusinessException("La penalización es obligatoria.");

            if (request.UsuarioResolucionId <= 0)
                throw new BusinessException("El usuario responsable de la resolución es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.MotivoResolucion))
                throw new BusinessException("El motivo de resolución es obligatorio.");

            var usuarioResponsable = await _usuarios.ObtenerporIdAsync(request.UsuarioResolucionId);

            if (usuarioResponsable is null)
                throw new BusinessException("El usuario responsable no existe.");

            if (usuarioResponsable.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario responsable no está activo.");

            if (usuarioResponsable is not Bibliotecario && usuarioResponsable is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede resolver penalizaciones.");

            var penalizacion = await _penalizaciones.ObtenerporIdAsync(request.IdPenalizacion);

            if (penalizacion is null)
                throw new BusinessException("La penalización no existe.");

            penalizacion.Resolver(
                request.UsuarioResolucionId,
                request.MotivoResolucion
            );

            await _penalizaciones.ActualizarAsync(penalizacion);

            await _auditoria.RegistrarAsync(
                request.UsuarioResolucionId,
                "Resolver penalización",
                "Penalizacion",
                $"Se resolvió la penalización ID {penalizacion.IdPenalizacion} del usuario ID {penalizacion.UsuarioId}."
            );

            return MapearPenalizacion(penalizacion);
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