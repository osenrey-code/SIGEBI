using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReportePrestamo
    {
        private readonly IRepositorioPrestamo _prestamos;

        public GenerarReportePrestamo(IRepositorioPrestamo prestamos)
        {
            _prestamos = prestamos;
        }

        public async Task<ResultadoOperacionResponse<ReportePrestamoResponse>> EjecutarAsync(
            GenerarReporteRequest request)
        {
            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value.Date > request.FechaFin.Value.Date)
            {
                return ResultadoOperacionResponse<ReportePrestamoResponse>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var prestamos = await _prestamos.ObtenerTodosAsync();

            if (request.FechaInicio.HasValue)
            {
                prestamos = prestamos.Where(p =>
                    p.FechaSolicitud.Date >= request.FechaInicio.Value.Date
                );
            }

            if (request.FechaFin.HasValue)
            {
                prestamos = prestamos.Where(p =>
                    p.FechaSolicitud.Date <= request.FechaFin.Value.Date
                );
            }

            var lista = prestamos.ToList();
            var fechaActual = DateTime.Now.Date;

            var prestamosDevueltos = lista.Where(p =>
                p.Estado == EstadoPrestamo.Devuelto &&
                p.FechaDevolucion.HasValue
            ).ToList();

            var devueltosATiempo = prestamosDevueltos.Count(p =>
                p.FechaLimite.HasValue &&
                p.FechaDevolucion!.Value.Date <= p.FechaLimite.Value.Date
            );

            var devueltosTarde = prestamosDevueltos.Count(p =>
                p.FechaLimite.HasValue &&
                p.FechaDevolucion!.Value.Date > p.FechaLimite.Value.Date
            );

            var tasaDevolucionPuntual = prestamosDevueltos.Count == 0
                ? 0
                : Math.Round(
                    (decimal)devueltosATiempo / prestamosDevueltos.Count * 100,
                    2
                );

            var response = new ReportePrestamoResponse
            {
                TotalPrestamos = lista.Count,

                PrestamosSolicitados = lista.Count(p =>
                    p.Estado == EstadoPrestamo.Solicitado
                ),

                PrestamosActivos = lista.Count(p =>
                    p.Estado == EstadoPrestamo.Activo
                ),

                PrestamosDevueltos = lista.Count(p =>
                    p.Estado == EstadoPrestamo.Devuelto
                ),

                PrestamosRechazados = lista.Count(p =>
                    p.Estado == EstadoPrestamo.Rechazado
                ),

                PrestamosVencidos = lista.Count(p =>
                    p.Estado == EstadoPrestamo.Vencido ||
                    (
                        p.Estado == EstadoPrestamo.Activo &&
                        p.FechaLimite.HasValue &&
                        p.FechaLimite.Value.Date < fechaActual
                    )
                ),

                PrestamosDevueltosATiempo = devueltosATiempo,
                PrestamosDevueltosTarde = devueltosTarde,
                TasaDevolucionPuntual = tasaDevolucionPuntual
            };

            return ResultadoOperacionResponse<ReportePrestamoResponse>.Ok(
                "Reporte de préstamos generado correctamente.",
                response
            );
        }
    }
}
