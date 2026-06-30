using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class RegistrarPrestamoPresencial
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;

        public RegistrarPrestamoPresencial(
            IUsuario usuarios,
            IRepositorioRecurso recursos,
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones)
        {
            _usuarios = usuarios;
            _recursos = recursos;
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            RegistrarPrestamoPresencialRequest request)
        {
            if (request.UsuarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario es obligatorio."
                );
            }

            if (request.RecursoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico es obligatorio."
                );
            }

            if (request.BibliotecarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            var bibliotecario = await _usuarios.ObtenerPorIdAsync(
                request.BibliotecarioId
            );

            if (bibliotecario is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            if (bibliotecario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            if (bibliotecario.Tipo != TipoUsuario.Bibliotecario &&
                bibliotecario.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo un bibliotecario o administrador puede registrar préstamos presenciales."
                );
            }

            var usuario = await _usuarios.ObtenerConPerfilAsync(
                request.UsuarioId
            );

            if (usuario is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no existe."
                );
            }

            if (usuario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no está activo."
                );
            }

            if (usuario.Tipo != TipoUsuario.Estudiante &&
                usuario.Tipo != TipoUsuario.Docente)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo estudiantes y docentes pueden recibir préstamos."
                );
            }

            if (usuario.PerfilLector is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no tiene perfil lector asignado."
                );
            }

            var penalizacionActiva = await _penalizaciones.ObtenerActivaPorPerfilLectorAsync(
                usuario.PerfilLector.Id
            );

            if (penalizacionActiva is not null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario tiene una penalización activa y no puede recibir préstamos."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(
                request.RecursoId
            );

            if (recurso is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico no existe."
                );
            }

            if (!recurso.EstaDisponible())
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico no está disponible para préstamo."
                );
            }

            var prestamosActivos = await _prestamos.ObtenerActivosPorPerfilLectorAsync(
                usuario.PerfilLector.Id
            );

            var cantidadPrestamosActivos = prestamosActivos.Count();

            if (!usuario.PerfilLector.PuedeSolicitarPrestamo(cantidadPrestamosActivos))
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    $"El usuario alcanzó el límite de préstamos permitidos. Préstamos activos actuales: {cantidadPrestamosActivos}."
                );
            }

            var prestamo = new Prestamo(
                usuario.PerfilLector.Id,
                recurso.Id
            );

            try
            {
                prestamo.AprobarYEntregar(
                    usuario.PerfilLector.DiasPrestamosPermitidos
                );

                recurso.MarcarComoPrestado();
            }
            catch (BusinessException ex)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    ex.Message
                );
            }

            await _prestamos.AgregarAsync(prestamo);
            await _recursos.ActualizarAsync(recurso);

            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Préstamo presencial registrado correctamente.",
                MapearPrestamo(prestamo)
            );
        }

        private static PrestamoResponse MapearPrestamo(Prestamo prestamo)
        {
            return new PrestamoResponse
            {
                PrestamoId = prestamo.Id,
                PerfilLectorId = prestamo.PerfilLectorId,
                RecursoId = prestamo.RecursoId,

                FechaSolicitud = prestamo.FechaSolicitud,
                FechaInicio = prestamo.FechaInicio,
                FechaLimite = prestamo.FechaLimite,
                FechaDevolucion = prestamo.FechaDevolucion,

                Estado = prestamo.Estado.ToString(),
                MotivoRechazo = prestamo.MotivoRechazo
            };
        }
    }
}
