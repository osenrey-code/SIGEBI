using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class AutenticarUsuario
    {
        private readonly IUsuario _usuarios;

        public AutenticarUsuario(IUsuario usuarios)
        {
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<LoginResponse>> EjecutarAsync(
            LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UsuarioOCorreo))
            {
                return ResultadoOperacionResponse<LoginResponse>.Error("El usuario o correo electronico es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(request.PassWord))
            {
                return ResultadoOperacionResponse<LoginResponse>.Error("La contraseña es obligatoria.");
            }

            var usuarioOCorreo = request.UsuarioOCorreo.Trim();

            Usuario? usuario;

            if (usuarioOCorreo.Contains("@"))
            {
                var usuarios = await _usuarios.ObtenerTodosAsync();

                usuario = usuarios.FirstOrDefault(u => u.Correo.Equals(usuarioOCorreo, StringComparison.OrdinalIgnoreCase));
            } else
            {
                usuario = await _usuarios.ObtenerPorIdentificacionAsync(usuarioOCorreo);
            }

            if (usuario is null)
            {
                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "Credenciales inválidas.");
            }

            if (usuario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<LoginResponse>.Error("El usuario no está activo.");
            }

            var response = new LoginResponse
            {
                UsuarioId = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = usuario.Tipo.ToString(),
                Token = ""
            };

            return ResultadoOperacionResponse<LoginResponse>.Ok(
                "Usuario autenticado correctamente.", response);
        }
    }
}
