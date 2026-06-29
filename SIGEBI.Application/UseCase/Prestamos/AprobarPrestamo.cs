using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class AprobarPrestamo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;

        public AprobarPrestamo(
            IRepositorioPrestamo prestamos,
            IRepositorioRecurso recursos,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _recursos = recursos;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            AprobarPrestamoRequest request)
        {
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            if (request.BibliotecarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            if (request.DiasPermitidos <= 0)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Los días permitidos deben ser mayores que cero."
                );
            }

            var bibliotecario = await _usuarios.ObtenerPorIdAsync(request.BibliotecarioId);

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
                    "Solo un bibliotecario o administrador puede aprobar préstamos."
                );
            }

            var prestamo = await _prestamos.ObtenerporIdAsync(request.PrestamoId);

            if (prestamo is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo no existe."
                );
            }

            if (prestamo.Estado != EstadoPrestamo.Solicitado)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo se pueden aprobar préstamos en estado Solicitado."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(prestamo.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico asociado al préstamo no existe."
                );
            }

            if (!recurso.EstaDisponible())
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico ya no está disponible."
                );
            }

            prestamo.AprobarYEntregar(request.DiasPermitidos);
            recurso.MarcarComoPrestado();

            await _prestamos.ActualizarAsync(prestamo);
            await _recursos.ActualizarAsync(recurso);

            var response = MapearPrestamo(prestamo);

            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Préstamo aprobado correctamente.",
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
