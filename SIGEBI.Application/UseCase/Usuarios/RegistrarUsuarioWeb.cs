using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class RegistrarUsuarioWeb
    {
        private readonly IUsuario _usuarios;

        public RegistrarUsuarioWeb(IUsuario usuarios)
        {
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<UsuarioResponse>> EjecutarAsync(
            RegistrarUsuarioWebRequest request)
        {
            var validacion = ValidarDatos(
                request.NombreCompleto,
                request.DocumentoIdentidad,
                request.Correo,
                request.PassWord,
                request.TipoUsuario
             );

            if (validacion is not  null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(validacion);
            }

            if (!Enum.TryParse<TipoUsuario>(request.TipoUsuario, true, out var tipoUsuario))
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "El tipo de usuario no es válido"
                    );
            }

            if (tipoUsuario != TipoUsuario.Estudiante && tipoUsuario != TipoUsuario.Docente)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "En el registro web solo se permiten usuarios Estudiante o Docente");
            }

            var usuarioExistente = await _usuarios.ObtenerPorIdentificacionAsync(
                request.DocumentoIdentidad.Trim());

            if (usuarioExistente is not null)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Ya existe un usuario con esa identificación.");
            }

            var correoExiste = await ExisteCorreoAsync(request.Correo.Trim());

            if (correoExiste)
            {
                return ResultadoOperacionResponse<UsuarioResponse>.Error(
                    "Ya existe un usuario con este correo.");
            }

            var usuario = new Usuario(
                request.DocumentoIdentidad.Trim(),
                request.NombreCompleto.Trim(),
                request.Correo.Trim(),
                tipoUsuario
             );

         
            var perfilLector = CrearPerfilLectorPorTipo(usuario.Id, tipoUsuario);
            usuario.AsignarPerfilLector(perfilLector);
            

            await _usuarios.AgregarAsync(usuario);
            var response = MapearUsuario(usuario);
            return ResultadoOperacionResponse<UsuarioResponse>.Ok("" +
                "Usuario registrado correctamente", response);

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

        private static string? ValidarDatos(
            string nombreCompleto,
            string documentoIdentidad,
            string correo,
            string password,
            string tipoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                return "El nombre completo es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(documentoIdentidad))
            {
                return "El documento de identidad es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(correo))
            { 
                return "El correo es obligatorio."; 
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return "La contraseña es obligatoria.";
            }

            if (string.IsNullOrWhiteSpace(tipoUsuario))
            {
                return "El tipo de usuario es obligatorio.";
            }

            return null;
        }

    }
}
