using SIGEBI.Application.Common;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;

namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReportesUsoCatalogo
    {
        private readonly IRepositorioReporte _reportes;
        private readonly ValidadorReportes _validador;

        public GenerarReportesUsoCatalogo(
            IRepositorioReporte reportes,
            ValidadorReportes validador)
        {
            _reportes = reportes;
            _validador = validador;
        }

        public async Task<ReporteUsoCatalogoResponse> EjecutarAsync(
            ReporteRangoFRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los filtros del reporte");

            await _validador.ValidarAdministradorOAuditorAsync(usuarioEjecutorId);

            ValidadorReportes.ValidarRangoFechas(
                request.FechaInicio,
                request.FechaFin
            );

            return await _reportes.ObtenerReporteUsoCatalogoAsync(
                request.FechaInicio,
                request.FechaFin
            );
        }
    }
}