using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class ConsultarPrestamoPorId
    {
        private readonly IRepositorioPrestamo _prestamos;

        public ConsultarPrestamoPorId(IRepositorioPrestamo prestamos)
        {
            _prestamos = prestamos;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            ConsultarPrestamosPorIdRequest request)
        {
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            var prestamo = await _prestamos.ObtenerporIdAsync(
                request.PrestamoId
            );

            if (prestamo is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo no existe."
                );
            }

            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Préstamo consultado correctamente.",
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
