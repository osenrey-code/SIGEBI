using SIGEBI.Application.Common;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;

namespace SIGEBI.Application.UseCase.Reportes
{
    public class GestionReportes : IGestionReportesUseCase
    {
        private readonly IRepositorioReporte _reportes;
        private readonly ValidadorReportes _validador;
        private readonly IExportadorReportePdf _exportadorPdf;

        public GestionReportes(
            IRepositorioReporte reportes,
            ValidadorReportes validador,
            IExportadorReportePdf exportadorPdf)
        {
            _reportes = reportes;
            _validador = validador;
            _exportadorPdf = exportadorPdf;
        }

        public async Task<ReporteInventarioResponse> GenerarReporteInventarioAsync(
            int usuarioEjecutorId)
        {
            await _validador.ValidarAccesoReporteInventarioAsync(
                usuarioEjecutorId
            );

            return await _reportes.ObtenerReporteInventarioAsync();
        }

        public async Task<byte[]> GenerarReporteInventarioPdfAsync(
            int usuarioEjecutorId)
        {
            var reporte = await GenerarReporteInventarioAsync(
                usuarioEjecutorId
            );

            return _exportadorPdf.GenerarReporteInventarioPdf(
                reporte
            );
        }

        public async Task<ReportePrestamoResponse> GenerarReportePrestamosAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los filtros del reporte");

            await _validador.ValidarAdministradorOAuditorAsync(
                usuarioEjecutorId
            );

            ValidadorReportes.ValidarRangoFechas(
                request.FechaInicio,
                request.FechaFin
            );

            return await _reportes.ObtenerReportePrestamoAsync(
                request.FechaInicio,
                request.FechaFin
            );
        }

        public async Task<byte[]> GenerarReportePrestamosPdfAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            var reporte = await GenerarReportePrestamosAsync(
                request,
                usuarioEjecutorId
            );

            return _exportadorPdf.GenerarReportePrestamosPdf(
                reporte,
                request
            );
        }

        public async Task<ReportePenalizacionesResponse> GenerarReportePenalizacionesAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los filtros del reporte");

            await _validador.ValidarAdministradorOAuditorAsync(
                usuarioEjecutorId
            );

            ValidadorReportes.ValidarRangoFechas(
                request.FechaInicio,
                request.FechaFin
            );

            return await _reportes.ObtenerReportePenalizacionesAsync(
                request.FechaInicio,
                request.FechaFin
            );
        }

        public async Task<byte[]> GenerarReportePenalizacionesPdfAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            var reporte = await GenerarReportePenalizacionesAsync(
                request,
                usuarioEjecutorId
            );

            return _exportadorPdf.GenerarReportePenalizacionesPdf(
                reporte,
                request
            );
        }

        public async Task<ReporteUsoCatalogoResponse> GenerarReporteUsoCatalogoAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los filtros del reporte");

            await _validador.ValidarAdministradorOAuditorAsync(
                usuarioEjecutorId
            );

            ValidadorReportes.ValidarRangoFechas(
                request.FechaInicio,
                request.FechaFin
            );

            return await _reportes.ObtenerReporteUsoCatalogoAsync(
                request.FechaInicio,
                request.FechaFin
            );
        }

        public async Task<byte[]> GenerarReporteUsoCatalogoPdfAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            var reporte = await GenerarReporteUsoCatalogoAsync(
                request,
                usuarioEjecutorId
            );

            return _exportadorPdf.GenerarReporteUsoCatalogoPdf(
                reporte,
                request
            );
        }
    }
}