using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;


namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReportePenalizaciones
    {
        private readonly IRepositorioPenalizacion _penalizaciones;

        public GenerarReportePenalizaciones(
            IRepositorioPenalizacion penalizaciones)
        {
            _penalizaciones = penalizaciones;
        }

        public async Task<ResultadoOperacionResponse<ReportePenalizacionesResponse>> EjecutarAsync(
            GenerarReporteRequest request)
        {
            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value.Date > request.FechaFin.Value.Date)
            {
                return ResultadoOperacionResponse<ReportePenalizacionesResponse>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var penalizaciones = await _penalizaciones.ObtenerTodosAsync();

            var lista = penalizaciones.ToList();

            var activas = lista.Where(p =>
                p.Estado == EstadoPenalizacion.Activa
            ).ToList();

            var resueltas = lista.Where(p =>
                p.Estado == EstadoPenalizacion.Resuelta
            ).ToList();

            var response = new ReportePenalizacionesResponse
            {
                TotalPenalizaciones = lista.Count,
                PenalizacionesActivas = activas.Count,
                PenalizacionesResueltas = resueltas.Count,

                TotalDiasRetraso = lista.Sum(p => p.DiasRetraso),

                MontoTotalMora = lista.Sum(p => p.MontoMora),
                MontoMoraActiva = activas.Sum(p => p.MontoMora),
                MontoMoraResuelta = resueltas.Sum(p => p.MontoMora)
            };

            return ResultadoOperacionResponse<ReportePenalizacionesResponse>.Ok(
                "Reporte de penalizaciones generado correctamente.",
                response
            );
        }
    }
}
