using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;

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

        public async Task<ResultadoOperacionResponse<UsuarioResponse>> EjecutarAsync(
            ActualizarUsuarioRequest request)
        {
            // Validamos quién ejecuta la acción.
            // Para auditoría necesitamos saber qué administrador actualizó el usuario.
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            // Validamos cuál usuario será actualizado.
            if (request.UsuarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El Id del usuario es obligatorio."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            // Un administrador inactivo no debe modificar usuarios.
            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Solo un administrador puede actualizar usuarios."
                );
            }

            var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId);

            if (usuario is null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El usuario no existe."
                );
            }

            if (string.IsNullOrWhiteSpace(request.NombreCompleto))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El nombre es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(request.Correo))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El correo es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(request.TipoUsuario))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El tipo de usuario es obligatorio."
                );
            }

            if (!Enum.TryParse<TipoUsuario>(
                    request.TipoUsuario,
                    ignoreCase: true,
                    out var tipoUsuario))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El tipo de usuario no es válido."
                );
            }

            var correoExiste = await ExisteCorreoEnOtroUsuarioAsync(
                request.Correo.Trim(),
                usuario.Id
            );

            if (correoExiste)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Ya existe otro usuario con ese correo."
                );
            }

            // Guardamos los valores anteriores antes de modificar.
            // Esto permite que auditoría muestre qué cambió.
            var valoresAnteriores =
                $"Nombre: {usuario.NombreCompleto}; " +
                $"Correo: {usuario.Correo}; " +
                $"Tipo: {usuario.Tipo}; " +
                $"Estado: {usuario.Estado}";

            usuario.NombreCompleto = request.NombreCompleto.Trim();
            usuario.Correo = request.Correo.Trim();
            usuario.Tipo = tipoUsuario;

            await _usuarios.ActualizarAsync(usuario);

            // Guardamos cómo quedó después de actualizar.
            var valoresNuevos =
                $"Nombre: {usuario.NombreCompleto}; " +
                $"Correo: {usuario.Correo}; " +
                $"Tipo: {usuario.Tipo}; " +
                $"Estado: {usuario.Estado}";

            // Registramos la actualización en auditoría.
            // El actor es el administrador que hizo la modificación.
            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Actualizar usuario",
                "Usuario",
                usuario.Id,
                "Exitoso",
                $"Se actualizó el usuario '{usuario.NombreCompleto}'.",
                valoresAnteriores,
                valoresNuevos
            );

            var response = MapearUsuario(usuario);

            return ResultadoOperacionResponse<UsuarioResponse>.Ok(
                "Usuario actualizado correctamente.",
                response
            );
        }

        private async Task<bool> ExisteCorreoEnOtroUsuarioAsync(
            string correo,
            Guid usuarioActualId)
        {
            var usuarios = await _usuarios.ObtenerTodosAsync();

            return usuarios.Any(u =>
                u.Id != usuarioActualId &&
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
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
