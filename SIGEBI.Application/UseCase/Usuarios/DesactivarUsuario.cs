using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;

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

        public async Task<ResultadoOperacionResponse> EjecutarAsync(
            DesactivarUsuarioRequest request)
        {
            var errorPermiso = await ValidarAdministradorAsync(
        request.UsuarioEjecutorId
    );

            if (errorPermiso is not null)
            {
                return ResultadoOperacionResponse.Error(errorPermiso);
            }

            if (request.UsuarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse.Error(
                    "El usuario a desactivar es obligatorio."
                );
            }

            if (request.UsuarioId == request.UsuarioEjecutorId)
            {
                return ResultadoOperacionResponse.Error(
                    "Un administrador no puede desactivarse a sí mismo."
                );
            }

            var usuario = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioId
            );

            if (usuario is null)
            {
                return ResultadoOperacionResponse.Error(
                    "El usuario no existe."
                );
            }

            if (usuario.Estado == EstadoUsuario.Inactivo)
            {
                return ResultadoOperacionResponse.Error(
                    "El usuario ya está inactivo."
                );
            }

            // Guardamos cómo estaba el usuario antes de desactivarlo.
            // Esto permite que la auditoría muestre el cambio realizado.
            var valoresAnteriores =
                $"Nombre: {usuario.NombreCompleto}; " +
                $"Correo: {usuario.Correo}; " +
                $"Tipo: {usuario.Tipo}; " +
                $"Estado: {usuario.Estado}";

            // Aplicamos la regla de dominio.
            usuario.Desactivar();

            // Guardamos el cambio en persistencia.
            await _usuarios.ActualizarAsync(usuario);

            // Guardamos cómo quedó el usuario después de desactivarlo.
            var valoresNuevos =
                $"Nombre: {usuario.NombreCompleto}; " +
                $"Correo: {usuario.Correo}; " +
                $"Tipo: {usuario.Tipo}; " +
                $"Estado: {usuario.Estado}";

            // Registramos la acción en auditoría.
            // El actor es el administrador que ejecutó la desactivación.
            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Desactivar usuario",
                "Usuario",
                usuario.Id,
                "Exitoso",
                $"Se desactivó el usuario '{usuario.NombreCompleto}'. Motivo: {request.Motivo}",
                valoresAnteriores,
                valoresNuevos
            );

            return ResultadoOperacionResponse.Ok(
                "Usuario desactivado correctamente."
            );
        }

        private async Task<string?> ValidarAdministradorAsync(Guid usuarioEjecutorId)
        {
            if (usuarioEjecutorId == Guid.Empty)
            {
                return "El usuario ejecutor es obligatorio.";
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                usuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return "El usuario ejecutor no existe.";
            }

            // Un administrador inactivo no debe poder ejecutar acciones administrativas.
            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return "El usuario ejecutor no está activo.";
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return "Solo un administrador puede desactivar usuarios.";
            }

            return null;
        }
    }
}

