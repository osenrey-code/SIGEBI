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

        public async Task<ReportePrestamoResponse> EjecutarAsync(
            ReporteRangoFRequest request)
        {
            return await _prestamos.ObtenerEstadisticaPrestamoAsync(request.FechaInicio, request.FechaFin);
        }
    }
}
