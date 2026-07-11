using SIGEBI.Application.Common;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;

namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReportePrestamo
    {
        private readonly IRepositorioReporte _reportes;
        private readonly ValidadorReportes _validador;
        private readonly IExportadorReportePdf _exportadorPdf;

        public GenerarReportePrestamo(IRepositorioReporte reportes,
            ValidadorReportes validador, IExportadorReportePdf exportador)
        {
            _reportes = reportes;
            _validador = validador;
            _exportadorPdf = exportador;
        }

        public async Task<ReportePrestamoResponse> EjecutarAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los filtros del reporte");

            await _validador.ValidarAdministradorOAuditorAsync(usuarioEjecutorId);

            ValidadorReportes.ValidarRangoFechas(
                request.FechaInicio,
                request.FechaFin
            );

            return await _reportes.ObtenerReportePrestamoAsync(
                request.FechaInicio,
                request.FechaFin
            );
        }

        public async Task<byte[]> EjecutarPdfAsync(
           ReporteRangoFRequest request,
           int usuarioEjecutorId)
        {
            var reporte = await EjecutarAsync(
                request,
                usuarioEjecutorId
            );

            return _exportadorPdf.GenerarReportePrestamosPdf(
                reporte,
                request
            );
        }
    }
}