using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext;

namespace SIGEBI.Application.UseCase.Usuarios
{
    public class AsignarPerfilLector
    {
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        public AsignarPerfilLector(IUsuario usuarios, IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<PerfilLectorResponse>> EjecutarAsync(
            AsignarPerfilLectorRequest request)
        {
            var errorPermiso = await ValidarAdministradorAsync(
                request.UsuarioEjecutorId
            );

            if (errorPermiso is not null)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    errorPermiso
                );
            }

            if (request.UsuarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    "El usuario es obligatorio."
                );
            }

            if (request.LimitePrestamos <= 0)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    "El límite de préstamos debe ser mayor que cero."
                );
            }

            if (request.DiasPrestamo <= 0)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    "Los días de préstamo deben ser mayores que cero."
                );
            }

            var usuario = await _usuarios.ObtenerConPerfilAsync(
                request.UsuarioId
            );

            if (usuario is null)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    "El usuario no existe."
                );
            }

            if (usuario.Tipo != TipoUsuario.Estudiante &&
                usuario.Tipo != TipoUsuario.Docente)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    "Solo los usuarios Estudiante o Docente pueden tener perfil lector."
                );
            }

            if (usuario.PerfilLector is not null)
            {
                return ResultadoOperacionResponse<PerfilLectorResponse>.Error(
                    "El usuario ya tiene un perfil lector asignado."
                );
            }

            var perfil = new PerfilLector(
                usuario.Id,
                request.LimitePrestamos,
                request.DiasPrestamo
            );

            usuario.AsignarPerfilLector(perfil);

            await _usuarios.ActualizarAsync(usuario);

            // Registramos la asignación del perfil lector.
            // Esto deja evidencia de quién asignó las condiciones de préstamo al usuario.
            await _auditoria.RegistrarAsync(
                request.UsuarioEjecutorId,
                "Asignar perfil lector",
                "Usuario",
                usuario.Id,
                "Exitoso",
                $"Se asignó perfil lector al usuario '{usuario.NombreCompleto}'.",
                valoresNuevos:
                    $"PerfilLectorId: {perfil.Id}; " +
                    $"LimitePrestamos: {perfil.LimitePrestamos}; " +
                    $"DiasPrestamo: {perfil.DiasPrestamosPermitidos}; " +
                    $"TipoUsuario: {usuario.Tipo}"
            );

            var response = new PerfilLectorResponse
            {
                PerfilLectorId = perfil.Id,
                UsuarioId = perfil.UsuarioId,
                LimitePrestamos = perfil.LimitePrestamos,
                DiasPrestamo = perfil.DiasPrestamosPermitidos,
                PrestamosActivos = 0
            };

            return ResultadoOperacionResponse<PerfilLectorResponse>.Ok(
                "Perfil lector asignado correctamente.",
                response
            );
        }

        private async Task<string?> ValidarAdministradorAsync(Guid usuarioEjecutorId)
        {
            if (usuarioEjecutorId == Guid.Empty)
            {
                return "El usuario ejecutor es obligatorio.";
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                usuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return "El usuario ejecutor no existe.";
            }

            // Un administrador inactivo no debe ejecutar acciones administrativas.
            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return "El usuario ejecutor no está activo.";
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Administrador)
            {
                return "Solo un administrador puede asignar perfiles lectores.";
            }

            return null;
        }
    }
}
