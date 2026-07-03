using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;


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
            var existe = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(request.Identificacion);
            if (existe == null) throw new BusinessException("No existe un usuario registrado con esta identifiación");

            var nombreAnterior = existe.NombreCompleto;
            existe.Actualizar(request.NombreCompleto);
            await _usuarios.ActualizarAsync(existe);

            await _auditoria.RegistrarAsync(
                UsuarioId: actorId,
                Accion: "Actualizar Usuario",
                EntidadAfectada: "Usuarios",
                detalles: $"Se actualizó el usuario({request.Identificacion}). Nombre anterior: '{nombreAnterior}', Nuevo nombre: '{existe.NombreCompleto}'"
            );

            return new UsuarioResponse
            {
                UsuarioId = existe.UsuarioId,
                Identificacion = request.Identificacion,
                NombreCompleto = existe.NombreCompleto,
                Correo = existe.Correo,
                TipoUsuario = existe.GetType().Name,
                Estado = existe.Estado.ToString()
            };
        } 
    }
}
