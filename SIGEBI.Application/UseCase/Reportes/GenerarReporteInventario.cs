using SIGEBI.Application.Common;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;

namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReporteInventario
    {
        private readonly IRepositorioReporte _reportes;
        private readonly ValidadorReportes _validador;
        private readonly IExportadorReportePdf _exportadorPdf;

        public GenerarReporteInventario(
            IRepositorioReporte reportes,
            ValidadorReportes validador, IExportadorReportePdf exportador)
        {
            _reportes = reportes;
            _validador = validador;
            _exportadorPdf = exportador;
        }

        public async Task<ReporteInventarioResponse> EjecutarAsync(
            int usuarioEjecutorId)
        {
 
            await _validador.ValidarAccesoReporteInventarioAsync(usuarioEjecutorId);
            return await _reportes.ObtenerReporteInventarioAsync();
        }

        public async Task<byte[]> EjecutarPdfAsync(int usuarioEjecutorId)
        {
            var reporte = await EjecutarAsync(usuarioEjecutorId);

            return _exportadorPdf.GenerarReporteInventarioPdf(reporte);
        }
    }
}