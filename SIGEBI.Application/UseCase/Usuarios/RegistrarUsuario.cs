using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class RegistrarUsuario
    {
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioPassword _password;

        public RegistrarUsuario(IUsuario usuarios, IAuditoriaService auditoria, IServicioPassword password)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
            _password = password;
        }

        public async Task<ResultadoOperacionResponse<UsuarioResponse>> EjecutarAsync(
            RegistrarUsuarioRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El usuario ejecutor es obligatorio."
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

            // Validamos que el administrador esté activo.
            // Un usuario inactivo no debería ejecutar acciones administrativas.
            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Solo un administrador puede registrar usuarios desde administración."
                );
            }

            var errorValidacion = ValidarDatosBasicos(
                request.NombreCompleto,
                request.DocumentoIdentidad,
                request.Correo,
                request.PassWord,
                request.TipoUsuario
            );

            if (errorValidacion is not null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    errorValidacion
                );
            }

            if (!Enum.TryParse<TipoUsuario>(request.TipoUsuario, true, out var tipoUsuario))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El tipo de usuario no es válido."
                );
            }

            var usuarioExistente = await _usuarios.ObtenerPorIdentificacionAsync(
                request.DocumentoIdentidad.Trim()
            );

            if (usuarioExistente is not null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Ya existe un usuario con esa identificación."
                );
            }

            var correoExiste = await ExisteCorreoAsync(request.Correo.Trim());

            if (correoExiste)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Ya existe un usuario con ese correo."
                );
            }

            var usuario = new Usuario(
                request.DocumentoIdentidad.Trim(),
                request.NombreCompleto.Trim(),
                request.Correo.Trim(),
                tipoUsuario
            );

            // Generamos el hash de la contraseña.
            // Nunca guardamos request.PassWord directamente.
            var passwordHash = _password.GenerarHash(
                request.PassWord.Trim()
            );

            usuario.EstablecerPasswordHash(passwordHash);

            if (tipoUsuario == TipoUsuario.Estudiante ||
                tipoUsuario == TipoUsuario.Docente)
            {
                var perfilLector = CrearPerfilLectorPorTipo(usuario.Id, tipoUsuario);

                usuario.AsignarPerfilLector(perfilLector);
            }

            await _usuarios.AgregarAsync(usuario);

            // Registramos la creación del usuario en auditoría.
            // No guardamos la contraseña en auditoría por seguridad.
            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Registrar usuario",
                "Usuario",
                usuario.Id,
                "Exitoso",
                $"Se registró el usuario '{usuario.NombreCompleto}' con tipo '{usuario.Tipo}'.",
                valoresNuevos:
                    $"Identificación: {usuario.Identificacion}; " +
                    $"Correo: {usuario.Correo}; " +
                    $"Tipo: {usuario.Tipo}; " +
                    $"Estado: {usuario.Estado}"
            );

            var response = MapearUsuario(usuario);

            return ResultadoOperacionResponse<UsuarioResponse>.Ok(
                "Usuario registrado correctamente desde administración.",
                response
            );
        }

        private async Task<bool> ExisteCorreoAsync(string correo)
        {
            var usuarios = await _usuarios.ObtenerTodosAsync();

            return usuarios.Any(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
        }

        private static PerfilLector CrearPerfilLectorPorTipo(
            Guid usuarioId,
            TipoUsuario tipoUsuario)
        {
            return tipoUsuario switch
            {
                TipoUsuario.Estudiante => new PerfilLector(
                    usuarioId,
                    3,
                    7
                ),

                TipoUsuario.Docente => new PerfilLector(
                    usuarioId,
                    5,
                    15
                ),

                _ => throw new Exception(
                    "Solo estudiantes y docentes pueden tener perfil lector."
                )
            };
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

        private static string? ValidarDatosBasicos(
            string nombreCompleto,
            string documentoIdentidad,
            string correo,
            string password,
            string tipoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return "El nombre completo es obligatorio.";

            if (string.IsNullOrWhiteSpace(documentoIdentidad))
                return "El documento de identidad es obligatorio.";

            if (string.IsNullOrWhiteSpace(correo))
                return "El correo es obligatorio.";

            if (string.IsNullOrWhiteSpace(password))
                return "La contraseña es obligatoria.";

            if (string.IsNullOrWhiteSpace(tipoUsuario))
                return "El tipo de usuario es obligatorio.";

            return null;
        }
    }
} 

