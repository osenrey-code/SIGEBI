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

        public async Task<ReportePenalizacionesResponse> EjecutarAsync(
            ReporteRangoFRequest request)
        {
            return await _penalizaciones.ObtenerEstadisticaPenalizacionesAsync(request.FechaInicio, request.FechaFin);
        }
    }
}
