using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class ActualizarUsuario
    {
        private readonly IUsuario _usuarios;

        public ActualizarUsuario(IUsuario usuarios)
        {
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<UsuarioResponse>> EjecutarAsync(
            ActualizarUsuarioRequest request)
        {
            if (request.UsuarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error("El Id del usuario es obligatorio.");
            }

            var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId);

            if (usuario is null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error("El usuario no existe.");
            }

            if (string.IsNullOrWhiteSpace(request.NombreCompleto))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(request.Correo))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error("El correo es obligatorio.");
            }

            if (!Enum.TryParse<TipoUsuario>(request.TipoUsuario, true, out var tipoUsuario))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error("El tipo de usuario no es válido.");
            }

            usuario.NombreCompleto = request.NombreCompleto.Trim();
            usuario.Correo = request.Correo.Trim();
            usuario.Tipo = tipoUsuario;

            await _usuarios.ActualizarAsync(usuario);
            var response = MapearUsuario(usuario);

            return ResultadoOperacionResponse<UsuarioResponse>.Ok("Usuario Actualizado correctamente.", response);

        }

        private static UsuarioResponse MapearUsuario(Usuario usuario)
        {
            return new UsuarioResponse
            {
                UsuarioId = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                DocumentoIdentidad = usuario.Identificacion,
                Correo = usuario.Correo,
                TipoUsuario = usuario.Tipo.ToString(),
                Estado = usuario.Estado.ToString()

            };
        }
    }
}
