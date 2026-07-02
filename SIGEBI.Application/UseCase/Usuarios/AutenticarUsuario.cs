using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

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
            // 1. Validaciones de entrada
            if (string.IsNullOrWhiteSpace(request.Identificacion))
                throw new BusinessException("La identificación es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new BusinessException("La contraseña es obligatoria.");

            // 2. Búsqueda por Identificación (Matrícula o Código de Empleado)
            var usuario = await _usuarios.ObtenerUsuarioPorIdentificacionAsync(request.Identificacion);

            // 3. Validaciones de negocio y seguridad
            if (usuario is null)
                throw new BusinessException("Credenciales inválidas.");

            if (usuario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("La cuenta de usuario no está activa.");

            if (string.IsNullOrWhiteSpace(usuario.PassWord))
                throw new BusinessException("El usuario no tiene credenciales configuradas.");

            // 4. Verificación de contraseña y auditoría de fallos
            var passwordValido = _servicioPassword.VerificarPassword(request.Password, usuario.PassWord);

            if (!passwordValido)
            {
                await _auditoria.RegistrarAsync(
                    UsuarioId: usuario.UsuarioId,
                    Accion: "Inicio de sesión fallido",
                    EntidadAfectada: "Usuarios",
                    detalles: $"Intento fallido con identificación: {request.Identificacion}"
                );

                throw new BusinessException("Credenciales inválidas.");
            }

            // 5. Generación de Token
            var tipoUsuarioReal = usuario.GetType().Name;

            var token = _servicioToken.GenerarToken(
                usuario.UsuarioId,
                usuario.NombreCompleto,
                usuario.Correo,
                tipoUsuarioReal
            );

            // 6. Auditoría de éxito
            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Inicio de sesión",
                EntidadAfectada: "Usuarios",
                detalles: $"El usuario '{usuario.NombreCompleto}' inició sesión correctamente."
            );

            // 7. Retorno del DTO de respuesta
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
