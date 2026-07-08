using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Reportes
{
     public class GenerarReportesUsoCatalogo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioRecurso _recursos;

        public GenerarReportesUsoCatalogo(
            IRepositorioPrestamo prestamos,
            IRepositorioRecurso recursos)
        {
            _prestamos = prestamos;
            _recursos = recursos;
        }

        public async Task<IEnumerable<ReporteUsoCatalogoResponse>> EjecutarAsync(
            ReporteRangoFRequest request)
        {
            return await _recursos.ObtenerEstadisticasUsoAsync(request.FechaInicio, request.FechaFin);
        }
    }
}
