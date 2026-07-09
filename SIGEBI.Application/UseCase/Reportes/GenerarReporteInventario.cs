using SIGEBI.Application.Common;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReporteInventario
    {
        private readonly IRepositorioReporte _reportes;
        private readonly ValidadorReportes _validador;

        public GenerarReporteInventario(
            IRepositorioReporte reportes,
            ValidadorReportes validador)
        {
            _reportes = reportes;
            _validador = validador;
        }

        public async Task<ReporteInventarioResponse> EjecutarAsync(
            int usuarioEjecutorId)
        {
            await _validador.ValidarAccesoReporteInventarioAsync(usuarioEjecutorId);

            return await _reportes.ObtenerReporteInventarioAsync();
        }
    }
}