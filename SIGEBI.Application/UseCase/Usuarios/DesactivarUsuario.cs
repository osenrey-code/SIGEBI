using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class DesactivarUsuario
    {
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        public DesactivarUsuario(IUsuario usuarios, IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

       
        public async Task DesactivarUsuarioAsync(DesactivarUsuarioRequest request, int actorId)
        {
            Guard.NotNull(request, "Los datos de desactivación");

            if (actorId <= 0)
                throw new BusinessException("El usuario que realiza la acción es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación del usuario");
            Guard.NotNullOrWhiteSpace(request.Motivo, "El motivo de desactivación");

            string identificacion = request.Identificacion.Trim();
            string motivo = request.Motivo.Trim();

            var actor = await _usuarios.ObtenerporIdAsync(actorId);

            if (actor is null)
                throw new BusinessException("El usuario que realiza la acción no existe.");

            if (actor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario que realiza la acción no está activo.");

            if (actor is not Administrador)
                throw new BusinessException("Solo un administrador puede desactivar usuarios.");

            var usuario = await _usuarios.ObtenerUsuarioConDetallesAsync(identificacion);

            if (usuario is null)
                throw new BusinessException("No existe un usuario registrado con esta identificación.");

            if (usuario.UsuarioId == actorId)
                throw new BusinessException("Un administrador no puede desactivarse a sí mismo.");

            if (usuario.Estado == EstadoUsuario.Inactivo)
                throw new BusinessException("El usuario ya se encuentra inactivo.");

            bool tienePrestamosActivos = usuario.Prestamos is not null &&
                usuario.Prestamos.Any(p => p.Estado == EstadoPrestamo.Activo);

            if (tienePrestamosActivos)
                throw new BusinessException("No se puede desactivar al usuario porque tiene préstamos activos pendientes de devolución.");

            usuario.Estado = EstadoUsuario.Inactivo;

            await _usuarios.ActualizarAsync(usuario);

            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Desactivar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se desactivó el usuario con identificación {identificacion}. Motivo: '{motivo}'."
            );
        }
    }
}


