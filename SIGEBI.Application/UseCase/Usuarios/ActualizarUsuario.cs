using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class ActualizarUsuario 
    {
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;

        public ActualizarUsuario(IUsuario usuarios, IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<UsuarioResponse> ActualizarUsuarioAsync(ActualizarUsuarioRequest request, int actorId)
        {
            Guard.NotNull(request, "Los datos del usuario");

            if (actorId <= 0)
                throw new BusinessException("El usuario que realiza la acción es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación del usuario");
            Guard.NotNullOrWhiteSpace(request.NombreCompleto, "El nombre completo del usuario");

            string identificacion = request.Identificacion.Trim();
            string nombreCompleto = request.NombreCompleto.Trim();

            var actor = await _usuarios.ObtenerporIdAsync(actorId);

            if (actor is null)
                throw new BusinessException("El usuario que realiza la acción no existe.");

            if (actor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario que realiza la acción no está activo.");

            if (actor is not Administrador)
                throw new BusinessException("Solo un administrador puede actualizar usuarios.");

            var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                identificacion
            );

            if (usuario is null)
                throw new BusinessException("No existe un usuario registrado con esta identificación.");

            string nombreAnterior = usuario.NombreCompleto;

            usuario.Actualizar(nombreCompleto);

            await _usuarios.ActualizarAsync(usuario);

            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Actualizar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se actualizó el usuario ({identificacion}). Nombre anterior: '{nombreAnterior}', nuevo nombre: '{usuario.NombreCompleto}'."
            );

            return MapearUsuario(usuario);
        }

        private static UsuarioResponse MapearUsuario(Usuario usuario)
        {
            return new UsuarioResponse
            {
                UsuarioId = usuario.UsuarioId,
                Identificacion = ObtenerIdentificacion(usuario),
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = usuario.GetType().Name,
                Estado = usuario.Estado.ToString()
            };
        }

        private static string ObtenerIdentificacion(Usuario usuario)
        {
            return usuario switch
            {
                Estudiante estudiante => estudiante.Matricula,
                Docente docente => docente.CodigoEmpleado,
                Administrador administrador => administrador.CodigoEmpleado,
                Bibliotecario bibliotecario => bibliotecario.CodigoEmpleado,
                Auditor auditor => auditor.CodigoEmpleado,
                _ => string.Empty
            };
        }
    }
}
