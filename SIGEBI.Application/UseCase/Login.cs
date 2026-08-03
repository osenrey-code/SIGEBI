using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase
{
    public class Login : ILogin
    {
        private readonly IUsuario _usuarios;
        private readonly IServicioPassword _password;
        private readonly IServicioToken _token;
        private readonly IAuditoriaService _auditoria;

        public Login(
            IUsuario usuarios,
            IServicioPassword password,
            IServicioToken token,
            IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _password = password;
            _token = token;
            _auditoria = auditoria;
        }

        public async Task<LoginResponse> AutenticarAsync(LoginRequest request)
        {
            Guard.NotNull(request, "Los datos de inicio de sesión");
            Guard.NotNullOrWhiteSpace(request.Identificacion, "La identificación");
            Guard.NotNullOrWhiteSpace(request.Password, "La contraseña");

            string identificacion = request.Identificacion.Trim();
            string password = request.Password.Trim();

            var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(
                identificacion
            );

            if (usuario is null)
                throw new BusinessException("Credenciales inválidas.");

            if (usuario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("La cuenta de usuario no está activa.");

            bool passwordValido = _password.VerificarPassword(
                password,
                usuario.PassWord
            );

            if (!passwordValido)
            {
                await _auditoria.RegistrarAsync(
                    UsuarioId: usuario.UsuarioId,
                    Accion: "Inicio de sesión fallido",
                    EntidadAfectada: "Usuarios",
                    detalles: $"Intento fallido con identificación: {identificacion}."
                );

                throw new BusinessException("Credenciales inválidas.");
            }

            string tipoUsuarioReal = usuario.GetType().Name;

            string token = _token.GenerarToken(
                usuario.UsuarioId,
                usuario.NombreCompleto,
                usuario.Correo,
                tipoUsuarioReal
            );

            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Inicio de sesión",
                EntidadAfectada: "Usuarios",
                detalles: $"El usuario '{usuario.NombreCompleto}' inició sesión correctamente."
            );

            return new LoginResponse
            {
                UsuarioId = usuario.UsuarioId,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                TipoUsuario = tipoUsuarioReal,
                Token = token
            };
        }
    }
}