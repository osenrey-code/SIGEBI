using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioReporte
    {
        Task<ReporteInventarioResponse> ObtenerReporteInventarioAsync();

        Task<ReporteUsoCatalogoResponse> ObtenerReporteUsoCatalogoAsync(
            DateTime fechaInicio,
            DateTime fechaFin
        );

        Task<ReportePenalizacionesResponse> ObtenerReportePenalizacionesAsync(
            DateTime fechaInicio,
            DateTime fechaFin
        );

        Task<ReportePrestamoResponse> ObtenerReportePrestamoAsync(
            DateTime fechaInicio,
            DateTime fechaFin
        );
    }
}
