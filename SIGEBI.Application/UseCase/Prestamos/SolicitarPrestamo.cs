using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using System.Runtime.CompilerServices;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class SolicitarPrestamo
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;

        public SolicitarPrestamo(
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
            SolicitarPrestamoRequest request)
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

            var usuario = await _usuarios.ObtenerConPerfilAsync(request.UsuarioId);

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
                    "Solo estudiantes y docentes pueden solicitar préstamos."
                );
            }

            if (usuario.PerfilLector is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no tiene un perfil lector asignado."
                );
            }

            var penalizacionActiva = await _penalizaciones.ObtenerActivaPorPerfilLectorAsync(
                usuario.PerfilLector.Id
            );

            if (penalizacionActiva is not null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario tiene una penalización activa y no puede solicitar préstamos."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoId);

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

            var prestamosActivos = await _prestamos.ObtenerActivosPorUsuarioAsync(
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

            await _prestamos.AgregarAsync(prestamo);

            var response = MapearPrestamo(prestamo);

            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Solicitud de préstamo registrada correctamente.",
                response
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
