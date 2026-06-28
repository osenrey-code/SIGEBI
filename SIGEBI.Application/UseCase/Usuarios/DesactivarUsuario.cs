using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class DesactivarUsuario
    {
        private readonly IUsuario _usuarios;
        public DesactivarUsuario(IUsuario usuarios)
        {
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse> EjecutarAsync(
            DesactivarUsuarioRequest request)
        {
            var errorPermiso = await ValidarAdministradorAsync(request.UsuarioEjecutorId);

            if (errorPermiso is not null)
                return ResultadoOperacionResponse.Error(errorPermiso);

            if (request.UsuarioId == Guid.Empty)
                return ResultadoOperacionResponse.Error("El usuario a desactivar es obligatorio.");

            if (request.UsuarioId == request.UsuarioEjecutorId)
            {
                return ResultadoOperacionResponse.Error(
                    "Un administrador no puede desactivarse a sí mismo."
                );
            }

            var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId);

            if (usuario is null)
                return ResultadoOperacionResponse.Error("El usuario no existe.");

            if (usuario.Estado == EstadoUsuario.Inactivo)
                return ResultadoOperacionResponse.Error("El usuario ya está inactivo.");

            usuario.Desactivar();

            await _usuarios.ActualizarAsync(usuario);

            return ResultadoOperacionResponse.Ok("Usuario desactivado correctamente.");
        }

        private async Task<string?> ValidarAdministradorAsync(Guid usuarioEjecutorId)
        {
            if (usuarioEjecutorId == Guid.Empty)
                return "El usuario ejecutor es obligatorio.";

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(usuarioEjecutorId);

            if (usuarioEjecutor is null)
                return "El usuario ejecutor no existe.";

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador)
                return "Solo un administrador puede desactivar usuarios.";

            return null;
        }
    }
}

