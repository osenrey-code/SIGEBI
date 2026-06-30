using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class AutenticarUsuario
    {
        private readonly IUsuario _usuarios;
        private readonly IServicioPassword _servicioPassword;
        private readonly IServicioToken _servicioToken;
        private readonly IAuditoriaService _auditoria;

        public AutenticarUsuario(IUsuario usuarios, IServicioPassword servicioPassword,
             IServicioToken servicioToken, IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _servicioPassword = servicioPassword;
            _servicioToken = servicioToken;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<LoginResponse>> EjecutarAsync(
            LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UsuarioOCorreo))
            {
                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "El usuario o correo electrónico es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(request.PassWord))
            {
                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "La contraseña es obligatoria."
                );
            }

            var usuarioOCorreo = request.UsuarioOCorreo.Trim();

            Usuario? usuario;

            if (usuarioOCorreo.Contains("@"))
            {
                var usuarios = await _usuarios.ObtenerTodosAsync();

                usuario = usuarios.FirstOrDefault(u =>
                    u.Correo.Equals(
                        usuarioOCorreo,
                        StringComparison.OrdinalIgnoreCase
                    )
                );
            }
            else
            {
                usuario = await _usuarios.ObtenerPorIdentificacionAsync(
                    usuarioOCorreo
                );
            }

            if (usuario is null)
            {
                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "Credenciales inválidas."
                );
            }

            if (usuario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "El usuario no está activo."
                );
            }

            if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
            {
                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "El usuario no tiene credenciales configuradas."
                );
            }

            var passwordValido = _servicioPassword.VerificarPassword(
                request.PassWord,
                usuario.PasswordHash
            );

            if (!passwordValido)
            {
                await _auditoria.RegistrarAsync(
                    usuario.Id,
                    "Inicio de sesión fallido",
                    "Usuario",
                    usuario.Id,
                    "Fallido",
                    "Intento de inicio de sesión con contraseña incorrecta."
                );

                return ResultadoOperacionResponse<LoginResponse>.Error(
                    "Credenciales inválidas."
                );
            }

            var token = _servicioToken.GenerarToken(
                usuario.Id,
                usuario.NombreCompleto,
                usuario.Correo,
                usuario.Tipo.ToString()
            );

            await _auditoria.RegistrarAsync(
                usuario.Id,
                "Inicio de sesión",
                "Usuario",
                usuario.Id,
                "Exitoso",
                $"El usuario '{usuario.NombreCompleto}' inició sesión correctamente."
            );

            var response = new LoginResponse
            {
                UsuarioId = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = usuario.Tipo.ToString(),
                Token = token
            };

            return ResultadoOperacionResponse<LoginResponse>.Ok(
                "Usuario autenticado correctamente.",
                response
            );
        }
    }
}
