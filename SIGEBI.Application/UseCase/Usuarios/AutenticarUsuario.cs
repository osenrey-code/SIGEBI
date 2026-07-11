using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;
using SIGEBI.Application.Interfaces.Service;

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

        public async Task<LoginResponse> AutenticarUsuarioAsync(LoginRequest request)
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

            bool passwordValido = _servicioPassword.VerificarPassword(
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

            string token = _servicioToken.GenerarToken(
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
