using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
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
            var usuario = await _usuarios.ObtenerUsuarioConDetallesAsync(request.Identificacion);

            if (usuario == null)
                throw new BusinessException("No existe un usuario registrado con esta identificación.");

            if (usuario.Estado == EstadoUsuario.Inactivo)
                throw new BusinessException("El usuario ya se encuentra inactivo.");

            // Validar que no tenga préstamos activos
            bool tienePrestamosActivos = usuario.Prestamos.Any(p => p.Estado == EstadoPrestamo.Activo);

            if (tienePrestamosActivos)
                throw new BusinessException("No se puede desactivar al usuario porque tiene préstamos activos pendientes de devolución.");

            usuario.Estado = EstadoUsuario.Inactivo;
            await _usuarios.ActualizarAsync(usuario);

            // Registrar la auditoría INCLUYENDO EL MOTIVO
            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Desactivar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se desactivó el usuario con identificación {request.Identificacion}. Motivo: '{request.Motivo}'"
            );
        }
    }
}

